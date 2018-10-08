using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// A collection of properties need to set up a connection
    /// to a data source, definition and 
    /// </summary>
    public class ProviderSetup
    {
        /// <summary>
        /// The identifier for the setup (which is also identified
        /// in the parent dictionary)
        /// </summary>
        public String Id { get; set; }

        /// <summary>
        /// The data definition needed for the provider
        /// </summary>
        public DataItemDefinition Definition { get; set; }

        /// <summary>
        /// The type of the data provider
        /// </summary>
        public Type ProviderType { get; set; }

        /// <summary>
        /// The connection string needed to set up the provider
        /// </summary>
        public String ConnectionString { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProviderSetup()
        {
        }
    }
}
