using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Definition of a property member of a data item
    /// </summary>
    [JsonObject]
    public class DataItemPropertyModel : DataItemPropertyBase
    {
        /// <summary>
        /// The data type for the property
        /// </summary>
        [JsonProperty]
        public String DataType { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public DataItemPropertyModel()
        {
            base.Initialise(); // Base initialiser
            DataType = typeof(String).ToString(); // String by default
        }
    }
}
