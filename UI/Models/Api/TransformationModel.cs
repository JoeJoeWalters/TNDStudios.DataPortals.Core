using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Definition of a transformation between two connections
    /// </summary>
    [JsonObject]
    public class TransformationModel : CommonObjectModel
    {
        /// <summary>
        /// Source Data Definition
        /// </summary>
        [JsonProperty]
        public KeyValuePair<Guid, String> Source { get; set; }

        /// <summary>
        /// Destination Data Definition
        /// </summary>
        [JsonProperty]
        public KeyValuePair<Guid, String> Destination { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TransformationModel() : base()
        {
            // Defaults
            Source = new KeyValuePair<Guid, String>();
            Destination = new KeyValuePair<Guid, String>();
        }
    }
}
