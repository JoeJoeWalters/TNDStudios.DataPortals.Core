using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Mapping to CommonObject object to transform the properties
    /// for rendering to Json etc.
    /// </summary>
    public class CommonObjectModel
    {
        /// <summary>
        /// The Id for this object
        /// </summary>
        [JsonProperty]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the definition
        /// </summary>
        [JsonProperty]
        public String Name { get; set; }

        /// <summary>
        /// The description of the definition
        /// </summary>
        [JsonProperty]
        public String Description { get; set; }
    }
}
