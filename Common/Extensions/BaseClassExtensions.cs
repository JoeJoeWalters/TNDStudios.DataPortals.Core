using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TNDStudios.DataPortals.Helpers
{
    /// <summary>
    /// Extensions to base classes to perform common actions
    /// </summary>
    public static class BaseClassExtensions
    {
        /// <summary>
        /// Returns a shortened string value suitable for comparison to data types
        /// </summary>
        /// <param name="value">The type value we want to convert to a short form</param>
        /// <returns>The string value representing the short form</returns>
        public static String ToShortName(this Type value)
        {
            // Split up the path of the object type
            String[] splitString = value.ToString().ToLower().Split('.');

            // Take the last element in the array as the object type
            return splitString[splitString.Length - 1];
        }

        // Capitalise the first character of a string
        public static String UppercaseFirst(this String s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// Convert and enumeration to a dictionary object
        /// </summary>
        /// <param name="value">The enumaration to convert</param>
        /// <returns>A dictionary object of the enumeration</returns>
        public static List<KeyValuePair<Int32, String>> ToList(this Type value)
        {
            List<KeyValuePair<Int32, String>> result =
                new List<KeyValuePair<Int32, String>>();

            // Loop the enumeration and get each value
            foreach (var item in Enum.GetValues(value))
            {
                // Do we have a description attribute?
                String description = ((Enum)item).GetEnumDescription();
                
                // Add the result to the return dictionary
                result.Add(new KeyValuePair<int, string>((Int32)item, description));
            }

            return result; // Send the dictionary back
        }

        /// <summary>
        /// Get the description attribute from an enum value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
