using System;
using System.Collections.Generic;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// Factory class to get the appropriate data provider
    /// Is static so that the providers can be stored to m
    /// </summary>
    public class DataProviderFactory
    {
        private Dictionary<Guid, IDataProvider> providers; // Data providers that have been set up 

        /// <summary>
        /// Get the provider type base on the string value provided
        /// </summary>
        /// <param name="provider">The provider to be resolved</param>
        /// <returns>The resolved data provider</returns>
        public IDataProvider Get(DataConnection connection) => Get(connection, null);
        public IDataProvider Get(
            DataConnection connection, 
            DataItemDefinition definition)
        {
            IDataProvider result = null; // Create a fail state by default

            // Do we already have a provider set up for this data connection
            Boolean existingProvider = providers.ContainsKey(connection.Id);
            if (!existingProvider)
            {
                // Decide on the type of object to create based on the enumeration
                // rather than storing the "type" in the object due to issues
                // with serialisation and portability
                Type type;
                switch (connection.ProviderType)
                {
                    case DataProviderType.FlatFileProvider:
                        type = typeof(FlatFileProvider);
                        break;
                    case DataProviderType.MSSQLProvider:
                        type = typeof(MSSQLProvider);
                        break;
                    default:
                        type = null;
                        break;
                }
                result = (IDataProvider)Activator.CreateInstance(type);
            }
            else
                result = providers[connection.Id];

            // Did we get a provider?
            if (result != null)
            {
                // Set the provider to be connected if it is not already connected
                if (!result.Connected && definition != null)
                    result.Connect(definition, connection.ConnectionString);

                // If the provider was not in the pooled collection of providers then add it
                if (!existingProvider)
                    providers[connection.Id] = result;
            }
            
            // Return the provider
            return result;
        }

        /// <summary>
        /// Initialise the provider factory
        /// </summary>
        public DataProviderFactory()
        {
            providers = new Dictionary<Guid, IDataProvider>() { };
        }

    }
}
