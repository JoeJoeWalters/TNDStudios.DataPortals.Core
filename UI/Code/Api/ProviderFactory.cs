using System;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.UI
{
    /// <summary>
    /// Factory class to get the appropriate data provider
    /// </summary>
    public class ProviderFactory
    {
        /// <summary>
        /// Get the provider type base on the string value provided
        /// </summary>
        /// <param name="provider">The provider to be resolved</param>
        /// <returns>The resolved data provider</returns>
        public IDataProvider Get(ProviderSetup setup)
        {
            IDataProvider result = null; // Create a fail state by default

            // Try and create an instance of the provider based on the type
            result = (IDataProvider)Activator.CreateInstance(setup.ProviderType);

            // Set the provider to be connected
            if (result != null)
                result.Connect(setup.Definition, setup.ConnectionString);

            // Return the provider
            return result;
        }
    }
}
