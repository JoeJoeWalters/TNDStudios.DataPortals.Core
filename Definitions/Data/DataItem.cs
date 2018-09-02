using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Core.Data
{
    /// <summary>
    /// Data item class to hold the data for a single entity of data
    /// </summary>
    public class DataItem
    {
        /// <summary>
        /// The definition of the data that the Data Item is holding
        /// </summary>
        public DataItemDefinition Definition { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataItem() =>
            Definition = new DataItemDefinition();

        /// <summary>
        /// Constructor with the definition being passed in
        /// </summary>
        /// <param name="definition">The initialising data definition</param>
        public DataItem(DataItemDefinition definition) =>
            Definition = definition;
    }
}
