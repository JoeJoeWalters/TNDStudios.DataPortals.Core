using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// The definition of a data item (How the properties of
    /// the data item are mapped etc.)
    /// </summary>
    public class DataItemDefinition : DataItemDefinitionBase
    {
        /// <summary>
        /// The specific culture information for this definition
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// The encoding format for the definition
        /// </summary>
        public Encoding EncodingFormat { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public DataItemDefinition() => 
            base.Initialise(new List<DataItemProperty>());

        /// <summary>
        /// Constructor with the property list being passed in
        /// </summary>
        /// <param name="properties">The initial list of properties</param>
        public DataItemDefinition(List<DataItemProperty> properties) =>
            base.Initialise(properties);

    }
}
