using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Helpers;

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
        DataPropertyTypes = 4
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
                    result = typeof(DataItemPropertyType)
                        .ToList()
                        .Select(item => 
                            new KeyValuePair<String,String>(item.Key.ToString(), item.Value)
                            )
                        .ToList();

                    break;
            }

            // Return the result
            return result;
        }
    }
}
