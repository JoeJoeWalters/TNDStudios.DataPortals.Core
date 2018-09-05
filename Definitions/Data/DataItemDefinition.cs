using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// The definition of a data item (How the properties of
    /// the data item are mapped etc.)
    /// </summary>
    public class DataItemDefinition
    {
        /// <summary>
        /// The list of properties that define the data item
        /// </summary>
        public List<DataItemProperty> Properties { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public DataItemDefinition() =>
            Properties = new List<DataItemProperty>();

        /// <summary>
        /// Constructor with the property list being passed in
        /// </summary>
        /// <param name="properties">The initial list of properties</param>
        public DataItemDefinition(List<DataItemProperty> properties) =>
            Properties = properties;
    }
}
