using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.PropertyBag;

namespace TNDStudios.DataPortals.PropertyBag
{
    /// <summary>
    /// Get the default property bag items for a given component of a given type
    /// </summary>
    public class PropertyBagFactory
    {
        /// <summary>
        /// Get the property bag from a given instance
        /// </summary>
        /// <returns>The property bag item types</returns>
        public List<PropertyBagItemType> Get(ObjectTypes objectType, Int32 value)
        {
            // The result
            List<PropertyBagItemType> result = new List<PropertyBagItemType>();

            // Check the enum type
            switch (objectType)
            {
                // We are checking data provider types
                case ObjectTypes.Connections:

                    IDataProvider provider = null;

                    DataProviderType dataProviderType = (DataProviderType)value;
                    switch (dataProviderType)
                    {
                        case DataProviderType.DelimitedFileProvider:
                            provider = new DelimitedFileProvider();
                            break;

                        case DataProviderType.FixedWidthFileProvider:
                            provider = new FixedWidthFileProvider();
                            break;

                        case DataProviderType.SQLProvider:
                            provider = new SQLProvider();
                            break;
                    }

                    // Did we get a provider?
                    if (provider != null)
                    {
                        result = provider.PropertyBagTypes(); // Get the property bag types
                        provider = null; // Destroy (effectively)
                    }

                    break;
            }

            // Return the result
            return result;
        }
    }
}
