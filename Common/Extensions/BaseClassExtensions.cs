using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
                String description = item.ToString();

                // Get the member information for the enum item
                var memInfo = value.GetMember(value.GetEnumName(item));
                if (memInfo.Length != 0)
                {
                    // Get the first description attribute
                    var descriptionAttribute = memInfo[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault() as DescriptionAttribute;

                    // We got something, assign the value
                    if (descriptionAttribute != null)
                        description = descriptionAttribute.Description;
                }

                // Add the result to the return dictionary
                result.Add(new KeyValuePair<int, string>((Int32)item, description));
            }

            return result; // Send the dictionary back
        }

    }
}
