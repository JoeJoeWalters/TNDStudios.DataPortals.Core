using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.Helpers
{
    /// <summary>
    /// Helpers to format data in to a specific pattern
    /// </summary>
    public class DataFormatHelper
    {
        /// <summary>
        /// Format some data to a specific patterm
        /// </summary>
        /// <param name="value">The value to be transformed</param>
        /// <param name="property">The property definition for the data (aka the column definition)</param>
        /// <param name="definition">The definitiion of the entire data set that this belongs to</param>
        /// <returns>The data formatted in a specific pattern and cast to a string</returns>
        public static String FormatData(Object value, DataItemProperty property, DataItemDefinition definition)
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

                        break;
                }
            }

            return result; // Send the formatted result back
        }
    }
}
