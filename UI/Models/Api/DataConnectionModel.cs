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
        /// The definitions that are assigned to this connection
        /// </summary>
        [JsonProperty]
        public List<KeyValuePair<Guid, String>> Definitions { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataConnectionModel() : base()
        {
            Definitions = new List<KeyValuePair<Guid, String>>(); // The definitions that are assigned to this connection
        }
    }
}
