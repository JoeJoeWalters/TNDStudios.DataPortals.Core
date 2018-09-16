using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// Definition of a property member of a data item
    /// </summary>
    public class DataItemProperty
    {
        /// <summary>
        /// Is this item the primary key?
        /// </summary>
        public Boolean PrimaryKey { get; set; }

        /// <summary>
        /// The "name" of the property (Generally used as the unique key)
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// A description of the property
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// The path for the property (E.g. when using XML/Json etc.)
        /// </summary>
        public String Path { get; set; }

        /// <summary>
        /// The oridinal position of the property when using flat files etc.
        /// </summary>
        public Int32 OridinalPosition { get; set; }

        /// <summary>
        /// The data type for the property
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// The pattern of the data (such as the date format etc.)
        /// </summary>
        public String Pattern { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public DataItemProperty()
        {
            PrimaryKey = false; // By default this is not the primary key
            Name = ""; // Empty String by default
            Description = ""; // Empty String by default
            Path = ""; // Empty String by default
            OridinalPosition = -1; // First item in the array by default
            DataType = typeof(String); // String by default
            Pattern = ""; // The pattern of the data (such as the date format)
        }
    }
}
