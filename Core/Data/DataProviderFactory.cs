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
#warning [This isn't the best approach but for small amounts of connections it's fine for now, use MD5 or other crytography has to get a larger Id later]
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

            // Did we get a provider?
            if (result != null)
            {
                // Set the provider to be connected if it is not already connected
                if (!result.Connected && definition != null)
                    result.Connect(definition, connection.ConnectionString);

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
