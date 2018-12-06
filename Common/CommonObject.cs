using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TNDStudios.DataPortals
{
    /// <summary>
    /// An enumeration to define the different object types within the system
    /// so it could be used for factory classes or just general classification
    /// </summary>
    public enum ObjectTypes
    {
        [Description("Api Definitions")]
        ApiDefinitions = 1,

        [Description("Data Definitions")]
        DataDefinitions = 2,

        [Description("Connections")]
        Connections = 3,

        [Description("Credentials")]
        Credentials = 4,

        [Description("Transformations")]
        Transformations = 5,

        [Description("Providers")]
        Providers = 6

    }

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
        /// The last time that this object was updated
        /// </summary>
        [JsonProperty]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CommonObject()
        {
            Id = Guid.NewGuid();
            Name = "";
            Description = "";
            LastUpdated = DateTime.Now; // Default value (Mainly used for refreshing caches)
        }
    }
}
