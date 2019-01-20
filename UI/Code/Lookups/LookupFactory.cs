using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Helpers;
using TNDStudios.DataPortals.PropertyBag;

namespace TNDStudios.DataPortals.UI
{
    /// <summary>
    /// Enumerations for lookups
    /// </summary>
    public enum LookupFactoryType : Int32
    {
        Unknown = 0,
        Encoding = 1,
        Culture = 2,
        DataTypes = 3,
        DataPropertyTypes = 4,
        DataItemPropertyBagItems = 5,
        ObjectTypes = 6,
        DataKeyTypes = 7
    }

    /// <summary>
    /// Factory class to handle getting a list of certain lookups
    /// </summary>
    public class LookupFactory
    {
        // List of data types that can be used by the system
        private readonly List<Type> dataTypes = new List<Type>()
        {
            typeof(Int32),
            typeof(Int64),
            typeof(String),
            typeof(DateTime),
            typeof(Double),
            typeof(Single),
            typeof(Boolean)
        };

        /// <summary>
        /// The enum to cast to a list type for the UI to consume
        /// </summary>
        /// <param name="enumType">The type to cast</param>
        /// <returns>A new list of keyvalue pairs for the UI to consume</returns>
        private List<KeyValuePair<String, String>> EnumToList(Type enumType)
            => enumType.ToList()
                .Select(item =>
                    new KeyValuePair<string, string>(item.Key.ToString(), item.Value)
                    ).ToList();

        /// <summary>
        /// Get a lookup of a given type
        /// </summary>
        /// <param name="type">The lookup type</param>
        /// <returns>The lookup dictionary</returns>
        public List<KeyValuePair<String, String>> Get(LookupFactoryType type)
        {
            // Result
            List<KeyValuePair<String, String>> result = new List<KeyValuePair<String, String>>();

            // Check the type to get the right lookup type
            switch (type)
            {
                case LookupFactoryType.Culture:

                    // Get the list of cultures available
                    result = CultureInfo.GetCultures(CultureTypes.AllCultures)
                        .Select(culture =>
                            new KeyValuePair<String, String>(culture.Name, culture.DisplayName)
                            ).OrderBy(column => column.Value).ToList();

                    break;

                case LookupFactoryType.Encoding:

                    // Get the list of encodings
                    result = Encoding.GetEncodings()
                        .Select(encoding =>
                            new KeyValuePair<String, String>(encoding.Name, encoding.DisplayName)
                            ).OrderBy(column => column.Value).ToList();

                    break;

                case LookupFactoryType.DataTypes:

                    // Get a list of data types with the appropriate title
                    result = dataTypes
                        .Select(dataType =>
                            new KeyValuePair<String, String>(
                                dataType.ToString(),
                                dataType.ToShortName().UppercaseFirst()
                                )
                            ).ToList(); // Return the data types constant

                    break;

                case LookupFactoryType.DataPropertyTypes:

                    // Cast the enumeration to a format that can be returned
                    result = EnumToList(typeof(DataItemPropertyType));

                    break;

                case LookupFactoryType.DataItemPropertyBagItems:

                    // Cast the enumeration to a format that can be returned
                    result = EnumToList(typeof(PropertyBagItemTypeEnum));

                    break;

                case LookupFactoryType.ObjectTypes:

                    // Cast the enumeration to a format that can be returned
                    result = EnumToList(typeof(ObjectTypes));

                    break;
                    
            }

            // Return the result
            return result;
        }
    }
}
