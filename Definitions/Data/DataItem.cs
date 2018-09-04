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
        /// The values associated with the data item
        /// </summary>
        public Dictionary<String, Object> Values { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataItem() =>
            Initialise();

        /// <summary>
        /// Common setup called by all constructorss
        /// </summary>
        /// <param name="definition">The definition of the data</param>
        private void Initialise()
        {
            Values = new Dictionary<string, object>();
        }
    }
}
