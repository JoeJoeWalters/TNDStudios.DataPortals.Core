using System;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Model for sending and receiving the data for a property bag item
    /// </summary>
    public class PropertyBagItemModel
    {
        /// <summary>
        /// The type of property bag item
        /// </summary
        public PropertyBagItemTypeModel ItemType { get; set; }

        /// <summary>
        /// The value for this property bag item
        /// </summary>
        public Object Value { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PropertyBagItemModel()
        {
            Value = null; // No value by default
            ItemType = new PropertyBagItemTypeModel(); // Default for the item type
        }
    }
}
