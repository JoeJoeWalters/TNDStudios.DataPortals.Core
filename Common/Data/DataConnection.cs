using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// Link between a data provider and a connection string that feeds it
    /// </summary>
    public class DataConnection : CommonObject
    {
        /// <summary>
        /// The type of provider for this data connection
        /// </summary>
        public DataProviderType ProviderType { get; set; }

        /// <summary>
        /// The connection string for this connection
        /// </summary>
        public String ConnectionString { get; set; }

        /// <summary>
        /// The definitions that are assigned to this connection
        /// </summary>
        public List<Guid> Definitions { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataConnection()
        {
            Definitions = new List<Guid>(); // The definitions that are assigned to this connection
        }
    }
}
