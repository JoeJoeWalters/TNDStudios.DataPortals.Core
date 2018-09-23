using System;
using System.Collections.Generic;
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
        /// Calculate the data type from a raw string value
        /// </summary>
        /// <param name="rawValue">A raw string value with some unknown data in it</param>
        /// <returns>The type that was derived from the raw string</returns>
        public static Type CalculateType(String rawValue)
        {
            if (Int32.TryParse(rawValue, out Int32 intValue))
                return typeof(Int32);
            else if (Int64.TryParse(rawValue, out Int64 bigintValue))
                return typeof(Int64);
            else if (double.TryParse(rawValue, out Double doubleValue))
                return typeof(Double);
            else if (booleanDetectStrings.Contains(rawValue.ToLower().Trim())
                || bool.TryParse(rawValue, out Boolean boolValue))
                return typeof(Boolean);
            else if (DateTime.TryParse(rawValue, out DateTime dateValue))
                return typeof(DateTime);
            else
                return typeof(String);
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
                String propertyType = property.DataType.ToString().ToLower().Replace("system.", "");

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
                        if (IsNumeric(value))
                            result = value;

                        break;

                    case "datetime":

                        // If a specific pattern has been provided then use that, otherwise use the culuture information provided
                        DateTime formattedDate = DateTime.MinValue;

                        // Do we have a manual property pattern or just leave it to the culture?
                        try
                        {
                            if ((property.Pattern ?? "") != "")
                                formattedDate = DateTime.ParseExact(value, (property.Pattern ?? ""), CultureInfo.InvariantCulture);
                            else
                                formattedDate = DateTime.Parse(value, definition.Culture);
                        }
                        catch { }

                        // Anything found? If so set it
                        if (formattedDate != DateTime.MinValue)
                            result = formattedDate;
                        else
                            result = DBNull.Value;


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
                String propertyType = property.DataType.ToString().ToLower().Replace("system.", "");
                switch (propertyType)
                {
                    case "boolean":
                    case "bool":

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

                        result = value.ToString();

                        break;

                    case "datetime":

                        DateTime dateTime = (DateTime)value; // Cast the date so we don't have to do it each time

                        // Do we have a manual property pattern or just leave it to the culture?
                        if ((property.Pattern ?? "") != "")
                            result = dateTime.ToString(property.Pattern);
                        else
                            result = dateTime.ToString(definition.Culture);

                        break;

                    default:

                        result = (String)value ?? "";

                        break;
                }
            }

            return result; // Send the formatted result back
        }
    }
}
