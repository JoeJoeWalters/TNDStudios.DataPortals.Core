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
        private Dictionary<String, IDataProvider> providers; // Data providers that have been set up 

        /// <summary>
        /// Generate a unique key for holding the provider
        /// (which could also include the definition)
        /// </summary>
        /// <returns></returns>
        public String GenerateConnectionKey(List<Guid> keys)
            => String.Join("-", keys.ToArray()).GetHashCode().ToString();
            
        /// <summary>
        /// Get the provider type base on the string value provided
        /// </summary>
        /// <param name="provider">The provider to be resolved</param>
        /// <returns>The resolved data provider</returns>
        public IDataProvider Get(DataConnection connection, Boolean addToCache) => Get(connection, null, addToCache);
        public IDataProvider Get(
            DataConnection connection, 
            DataItemDefinition definition,
            Boolean addToCache)
        {
            IDataProvider result = null; // Create a fail state by default

            // Do we already have a provider set up for this data connection
            String uniqueKey = GenerateConnectionKey(
                new List<Guid>()
                {
                    connection.Id,
                    ((definition == null || definition.Id == null) ? Guid.Empty : definition.Id)
                });

            Boolean existingProvider = providers.ContainsKey(uniqueKey);
            if (!existingProvider)
            {
                // Decide on the type of object to create based on the enumeration
                // rather than storing the "type" in the object due to issues
                // with serialisation and portability
                Type type;
                switch (connection.ProviderType)
                {
                    case DataProviderType.DelimitedFileProvider:
                        type = typeof(DelimitedFileProvider);
                        break;
                    case DataProviderType.FixedWidthFileProvider:
                        type = typeof(FixedWidthFileProvider);
                        break;
                    case DataProviderType.SQLProvider:
                        type = typeof(SQLProvider);
                        break;
                    default:
                        type = null;
                        break;
                }

                // Did we actually get a type?
                if (type != null)
                    result = (IDataProvider)Activator.CreateInstance(type);
                else
                    result = null;
            }
            else
                result = providers[uniqueKey];

            // If the provider is stale (something configuration-wise has been changed)
            if (result != null &&
                result.LastAction <= connection.LastUpdated)
            {

            }

            // Did we get a provider?
            if (result != null)
            {
                // Set the provider to be connected if it is not already connected
                // or if the provider has an updated definition
                if ((!result.Connected || (result.LastAction <= definition.LastUpdated))
                    && definition != null)
                {
                    result.Connect(definition, connection); // Connect attempt
                    existingProvider = false; // Re-connected potentially so reset to make sure it's added to the cache
                }

                // If the provider was not in the pooled collection of providers then add it
                if (!existingProvider && addToCache)
                    providers[uniqueKey] = result;
            }
            
            // Return the provider
            return result;
        }

        /// <summary>
        /// Initialise the provider factory
        /// </summary>
        public DataProviderFactory()
        {
            providers = new Dictionary<String, IDataProvider>() { };
        }

    }
}
