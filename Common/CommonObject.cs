using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals
{
    /// <summary>
    /// Common object properties for items that may be saved
    /// out to a system or need descriptions etc.
    /// </summary>
    public class CommonObject
    {
        /// <summary>
        /// The identifier of this object
        /// </summary>
        [JsonProperty]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of this object
        /// </summary>
        [JsonProperty]
        public String Name { get; set; }

        /// <summary>
        /// The description of this object
        /// </summary>
        [JsonProperty]
        public String Description { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CommonObject()
        {
            Id = Guid.NewGuid();
            Name = "";
            Description = "";
        }
    }
}
