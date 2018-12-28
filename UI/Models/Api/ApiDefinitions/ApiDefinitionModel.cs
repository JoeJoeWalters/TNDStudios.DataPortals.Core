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
        /// List of credentials that are allowed access to this API and how they 
        /// are configured for reading etc.
        /// </summary>
        [JsonProperty]
        public List<CredentialsLinkModel> CredentialsLinks { get; set; }

        /// <summary>
        /// List of property aliases (as the raw definition might want to be overridden)
        /// </summary>
        [JsonProperty]
        public List<KeyValuePair<String, String>> Aliases { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApiDefinitionModel() : base()
        {
            DataDefinition = new KeyValuePair<Guid, String>(Guid.Empty, ""); // No link by default
            DataConnection = new KeyValuePair<Guid, String>(Guid.Empty, ""); // No link by default
            CredentialsLinks = new List<CredentialsLinkModel>(); // No credential links by default
            Aliases = new List<KeyValuePair<String, String>>(); // No aliases by default
        }
    }
}
