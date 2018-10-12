using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Define how an API Endpoint 
    /// </summary>
    public class ApiDefinitionModel : CommonObjectModel
    {
        /// <summary>
        /// Pointer to the definition to use
        /// </summary>
        [JsonProperty]
        public KeyValuePair<Guid, String> DataDefinition { get; set; }

        /// <summary>
        /// Pointer to the data connection to use
        /// </summary>
        [JsonProperty]
        public KeyValuePair<Guid, String> DataConnection { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApiDefinitionModel() : base()
        {

        }
    }
}
