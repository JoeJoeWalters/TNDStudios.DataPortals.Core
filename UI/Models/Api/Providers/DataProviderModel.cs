using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using TNDStudios.DataPortals.Helpers;
using TNDStudios.DataPortals.PropertyBag;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// API based view of a given data provider's properties 
    /// (The base properties they all share)
    /// </summary>
    public class DataProviderModel
    {
        /// <summary>
        /// Can this provider read (as a data source)
        /// </summary>
        [JsonProperty]
        public Boolean CanRead { get; set; }

        /// <summary>
        /// Can this provider write (as a data destination)
        /// </summary>
        [JsonProperty]
        public Boolean CanWrite { get; set; }

        /// <summary>
        /// Can this provider expose analysis services (to reveal the 
        /// structure of the object being read)
        /// </summary>
        [JsonProperty]
        public Boolean CanAnalyse { get; set; }

        /// <summary>
        /// Can this provider list objects to connect to 
        /// </summary>
        [JsonProperty]
        public Boolean CanList { get; set; }

        /// <summary>
        /// The property bag types that can be used to define this connection
        /// </summary>
        [JsonProperty]
        public virtual List<PropertyBagItemTypeModel> PropertyBagTypes { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataProviderModel()
        {
        }
    }
}
