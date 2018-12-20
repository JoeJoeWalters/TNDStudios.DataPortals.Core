using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Link between a data provider and a connection string that feeds it
    /// </summary>
    [JsonObject]
    public class DataConnectionModel : CommonObjectModel
    {
        /// <summary>
        /// The type of provider for this data connection
        /// </summary>
        [JsonProperty]
        public Int32 ProviderType { get; set; }

        /// <summary>
        /// The connection string for this connection
        /// </summary>
        [JsonProperty]
        public String ConnectionString { get; set; }
        
        /// <summary>
        /// Which set of credentials to use to transpose security information
        /// in to connections strings etc.
        /// </summary>
        [JsonProperty]
        public KeyValuePair<Guid, String> Credentials { get; set; }

        /// <summary>
        /// The name of the object in the connection that the connection
        /// is pointing to (such as the name of the table or spreadsheet)
        /// </summary>
        public String ObjectName { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataConnectionModel() : base()
        {
            ProviderType = 0; // No provider type by default
            ConnectionString = String.Empty; // No connection string by default
            Credentials = new KeyValuePair<Guid, String>(Guid.Empty, ""); // No credentials by default
            ObjectName = String.Empty;
        }
    }
}
