using System;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Contains definitions of what property bag items can do
    /// </summary>
    public class PropertyBagItemTypeModel
    {
        /// <summary>
        /// Enumeration constant on the property type
        /// </summary>
        public Int32 PropertyType { get; set; }

        /// <summary>
        /// The data type of the property bag type
        /// </summary>
        public String DataType { get; set; }

        /// <summary>
        /// The default value for the property bag item type
        /// </summary>
        public Object DefaultValue { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public PropertyBagItemTypeModel()
        {
            PropertyType = 0; // Default enum value
            DataType = "String"; // String by default
            DefaultValue = String.Empty; // Empty String by default
        }
    }
}
