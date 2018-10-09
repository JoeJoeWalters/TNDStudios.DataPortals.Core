using System;
using System.Collections.Generic;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.UI
{
    /// <summary>
    /// Factory class to get the appropriate data provider
    /// Is static so that the providers can be stored to m
    /// </summary>
    public class ProviderFactory
    {
        private Dictionary<String, IDataProvider> providers; // Data providers that have been set up 

        /// <summary>
        /// Get the provider type base on the string value provided
        /// </summary>
        /// <param name="provider">The provider to be resolved</param>
        /// <returns>The resolved data provider</returns>
        public IDataProvider Get(ProviderSetup setup)
        {
            IDataProvider result = null; // Create a fail state by default

            // Do we already have a provider set up for this data connection
            Boolean existingProvider = providers.ContainsKey(setup.Id);
            if (!existingProvider)
                result = (IDataProvider)Activator.CreateInstance(setup.ProviderType);
            else
                result = providers[setup.Id];

            // Did we get a provider?
            if (result != null)
            {
                // Set the provider to be connected if it is not already connected
                if (!result.Connected)
                    result.Connect(setup.Definition, setup.ConnectionString);

                // If the provider was not in the pooled collection of providers then add it
                if (!existingProvider)
                    providers[setup.Id] = result;
            }
            
            // Return the provider
            return result;
        }

        /// <summary>
        /// Initialise the provider factory
        /// </summary>
        public ProviderFactory()
        {
            providers = new Dictionary<String, IDataProvider>() { };
        }

    }
}
