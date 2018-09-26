using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Web friendly model to represent the Data Item Definition class
    /// where the "Culture" and various other data types are not suited
    /// for sending back over the API calls
    /// </summary>
    public class DataItemDefinitionModel : DataItemDefinitionBase
    {
        /// <summary>
        /// The specific culture information for this definition
        /// </summary>
        public String Culture { get; set; }

        /// <summary>
        /// The encoding format for the definition
        /// </summary>
        public String EncodingFormat { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataItemDefinitionModel()
            => base.Initialise(new List<DataItemProperty>() { });

        public DataItemDefinitionModel(List<DataItemProperty> properties)
            => base.Initialise(properties);
    }
}
