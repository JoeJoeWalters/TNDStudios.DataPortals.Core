using System;
namespace TNDStudios.DataPortals.PropertyBag
{
    /// <summary>
    /// Contains definitions of what property bag items can do
    /// </summary>
    public class PropertyBagItemType
    {
        /// <summary>
        /// Enumeration constant on the property type
        /// </summary>
        public PropertyBagItemTypeEnum PropertyType { get; set; }

        /// <summary>
        /// The data type of the property bag type
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// The default value for the property bag item type
        /// </summary>
        public Object DefaultValue { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public PropertyBagItemType()
        {
            PropertyType = PropertyBagItemTypeEnum.Unknown; // Default enum value
            DataType = typeof(String); // String by default
            DefaultValue = String.Empty; // Empty String by default
        }
    }
}
