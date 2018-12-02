using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Definition of a set of data
    /// </summary>
    [JsonObject]
    public class DataItemValuesModel
    {
        /// <summary>
        /// List of line containing the key value pairs
        /// </summary>
        [JsonProperty]
        public List<Dictionary<String, String>> Lines { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataItemValuesModel()
        {
            Lines = new List<Dictionary<String, String>>(); // Create a default blank array
        }
    }
}
