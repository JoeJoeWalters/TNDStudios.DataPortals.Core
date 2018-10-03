﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.Helpers
{
    /// <summary>
    /// Helpers to format data in to a specific pattern
    /// </summary>
    public class DataFormatHelper
    {
        /// <summary>
        /// Constants that identify the character of a string as being 
        /// a positive boolean value
        /// </summary>
        private const String booleanChars = "1ytf";
        private static readonly List<String> booleanDetectStrings = 
            new List<String>()
            {
                "true", "yes", "1", "y", "false", "no", "0", "n"
            };

        /// <summary>
        /// Handle getting the most appropriate culture of a given data type
        /// </summary>
        /// <typeparam name="T">The type of data expected to be passed in (but passed in as a string representation)</typeparam>
        /// <param name="data">The data we need to detect the culture info for</param>
        /// <returns>The culture information recognised or null if none could be determined (or none required)</returns>
        public static CultureInfo FieldCulture<T>(String data, CultureInfo defaultCulture)
        {
            CultureInfo response = defaultCulture;

            // What type of data is this?
            switch (typeof(T).ToShortName())
            {
                // Date Time Data Type
                case "datetime":

                    DateTime castDate = DateTime.MinValue;

                    try
                    {
                        castDate = DateTime.Parse(data, defaultCulture);
                    }
                    catch {}

                    if (castDate != DateTime.MinValue)
                    {

                    }

                    break;
            }

            // Return the culture
            return response;
        }

        /// <summary>
        /// Calculate the data type from a raw string value
        /// </summary>
        /// <param name="rawValue">A raw string value with some unknown data in it</param>
        /// <returns>The type that was derived from the raw string</returns>
        public static Type CalculateType(String rawValue)
        {
            if (Int32.TryParse(rawValue, out Int32 intValue))
                return typeof(Int32);
            else if (double.TryParse(rawValue, out Double doubleValue))
                return typeof(Double);
            else if (Int64.TryParse(rawValue, out Int64 bigintValue))
                return typeof(Int64);
            else if (booleanDetectStrings.Contains(rawValue.ToLower().Trim())
                || bool.TryParse(rawValue, out Boolean boolValue))
                return typeof(Boolean);
            else if (IsGlobalDate(rawValue))
                return typeof(DateTime);
            else
                return typeof(String);
        }

        /// <summary>
        /// Determines whether a string is a date formatted string
        /// by comparing various cultures (with the current one first)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean IsGlobalDate(String value)
        {
            // List of cultures to test in by commonality
            List<CultureInfo> testCultures = new List<CultureInfo>()
            {
                CultureInfo.CurrentCulture,
                CultureInfo.GetCultureInfo("en-US"),
                CultureInfo.GetCultureInfo("en-GB"),
                CultureInfo.InvariantCulture
            };

            Boolean result = false;
            foreach (CultureInfo culture in testCultures)
            {
                try
                {
                    DateTime casted = DateTime.Parse(value, culture);
                    result = true;
                    break;
                }
                catch { }
            }

            return result;
        }

        /// <summary>
        /// Read some data from a specific pattern
        /// </summary>
        /// <param name="value">The value to read the data from</param>
        /// <param name="property">The property definition for the data (aka the column definition)</param>
        /// <param name="definition">The definitiion of the entire data set that this belongs to</param>
        /// <returns>The data formatted as the appropriate type</returns>
        public static Object ReadData(String value, DataItemProperty property, DataItemDefinition definition)
        {
            Object result = DBNull.Value; // The formatted result that will be returned

            // Get the property type as some types of data need handling differently straight away
            if (value != null)
            {
                String propertyType = property.DataType.ToShortName();
                switch (propertyType)
                {
                    case "boolean":
                    case "bool":

                        // Get the first character of the raw data if there is some
                        Char firstChar =
                            (value.Length > 0) ? value.ToCharArray()[0] : ' ';

                        // Check the first character to see if it matches a true state
                        result = booleanChars.Contains(firstChar);

                        break;

                    case "double":
                    case "int":
                    case "unint":
                    case "int16":
                    case "int32":
                    case "int64":
                    case "float":
                    case "decimal":
                    case "single":
                    case "byte":
                    case "sbyte":
                    case "short":
                    case "ushort":
                    case "long":
                    case "ulong":

                        // Clean the string up for parsing
                        try
                        {
                            if (IsNumeric(value))
                                result = value;
                        }
                        catch
                        {
                            result = (Int32)0;
                        }
                        
                        break;

                    case "datetime":

                        // If a specific pattern has been provided then use that, otherwise use the culuture information provided
                        DateTime formattedDate = DateTime.MinValue;

                        // Do we have a manual property pattern or just leave it to the culture?
                        try
                        {
                            #warning [In Debug mode this takes a long time to run due to the debugger trapping the data format errors when dates are in the incorrect format]
                            if ((property.Pattern ?? "") != "")
                                formattedDate = DateTime.ParseExact(value, (property.Pattern ?? ""), CultureInfo.InvariantCulture);
                            else
                                formattedDate = DateTime.Parse(value, definition.Culture);
                            
                            // Anything found? If so set it
                            if (formattedDate != DateTime.MinValue)
                                result = formattedDate;
                            else
                                result = DBNull.Value;
                        }
                        catch
                        {
                        }

                        break;

                    default:

                        result = value;

                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Check to see if a string is numeric (to wrap custom handlers)
        /// </summary>
        /// <param name="value">The string to check against</param>
        /// <returns>If the string is numeric or not</returns>
        public static Boolean IsNumeric(String value) =>
            int.TryParse(value, out int chuckInt) ||
            float.TryParse(value, out float chuckFloat);

        /// <summary>
        /// Clean out a piece of data so it can be handled manually without possible quotes etc.
        /// </summary>
        /// <param name="value">The raw value</param>
        /// <param name="csvReader">The reader that holds the configuration</param>
        /// <returns></returns>
        public static String CleanString(String value, Char quoteChar)
            => RemoveEnds(value, quoteChar).Trim().Replace("\"\"", "\"");

        /// <summary>
        /// Remove the character from the start and/or end of the string
        /// but not in the middle
        /// </summary>
        /// <param name="value"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public static String RemoveEnds(String value, Char character)
        {
            // Split up in to an array of characters
            value = (value ?? "").Trim(); // Handle any incoming nulls
            if (value.Length >= 2)
            {
                if (value.StartsWith(character) && value.EndsWith(character))
                {
                    value = value.Remove(0, 1);
                    value = value.Remove(value.Length - 1, 1);
                }
            }

            return value; // Return the formatted string
        }

        /// <summary>
        /// Write some data to a specific patterm
        /// </summary>
        /// <param name="value">The value to be transformed</param>
        /// <param name="property">The property definition for the data (aka the column definition)</param>
        /// <param name="definition">The definitiion of the entire data set that this belongs to</param>
        /// <returns>The data formatted in a specific pattern and cast to a string</returns>
        public static String WriteData(Object value, DataItemProperty property, DataItemDefinition definition)
        {
            String result = ""; // The formatted result that will be returned

            // Get the property type as some types of data need handling differently straight away
            if (value != null)
            {
                String propertyType = property.DataType.ToShortName();
                switch (propertyType)
                {
                    case "boolean":
                    case "bool":

                        try
                        {
                            result = Boolean.Parse(value.ToString()).ToString();
                        }
                        catch
                        {
                            result = "0";
                        }

                        break;

                    case "double":
                    case "int":
                    case "unint":
                    case "int16":
                    case "int32":
                    case "int64":
                    case "float":
                    case "decimal":
                    case "single":
                    case "byte":
                    case "sbyte":
                    case "short":
                    case "ushort":
                    case "long":
                    case "ulong":

                        try
                        {
                            result = value.ToString();
                        }
                        catch
                        {
                            result = "0";
                        }

                        break;

                    case "datetime":

                        // Not a null value
                        try
                        {
                            if (value != DBNull.Value)
                            {
                                DateTime dateTime = (DateTime)value; // Cast the date so we don't have to do it each time

                                // Do we have a manual property pattern or just leave it to the culture?
                                if ((property.Pattern ?? "") != "")
                                    result = dateTime.ToString(property.Pattern);
                                else
                                    result = dateTime.ToString(definition.Culture);
                            }
                            else
                                result = "";
                        }
                        catch
                        {
                            result = "";
                        }

                        break;

                    default:

                        try
                        {
                            result = (String)value ?? "";
                        }
                        catch
                        {
                            result = "";
                        }

                        break;
                }
            }

            return result; // Send the formatted result back
        }
    }
}
