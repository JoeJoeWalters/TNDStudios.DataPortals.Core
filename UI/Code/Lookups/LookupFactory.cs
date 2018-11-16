using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNDStudios.DataPortals.UI
{
    /// <summary>
    /// Enumerations for lookups
    /// </summary>
    public enum LookupFactoryType : Int32
    {
        Unknown = 0,
        Encoding = 1,
        Culture = 2
    }

    /// <summary>
    /// Factory class to handle getting a list of certain lookups
    /// </summary>
    public class LookupFactory
    {
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
                            ).ToList();

                    break;

                case LookupFactoryType.Encoding:

                    // Get the list of encodings
                    result = Encoding.GetEncodings()
                        .Select(encoding =>
                            new KeyValuePair<String, String>(encoding.Name, encoding.DisplayName)
                        ).ToList();

                    break;
            }

            // Return the result
            return result;
        }
    }
}
