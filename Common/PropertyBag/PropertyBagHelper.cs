using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TNDStudios.DataPortals.PropertyBag
{
    public class PropertyBagHelper
    {
        /// <summary>
        /// Property Bag Items referenced by the calling object
        /// </summary>
        private List<PropertyBagItem> propertyBagItems { get; set; }

        /// <summary>
        /// Constructor that takes the list of items from the calling object
        /// </summary>
        /// <param name="propertyBag"></param>
        public PropertyBagHelper(List<PropertyBagItem> propertyBag)
            => propertyBagItems = propertyBag;

        /// <summary>
        /// Get a configuration item from the property bag
        /// and format it as appropriate
        /// </summary>
        /// <typeparam name="T">The type of data that is requested</typeparam>
        /// <param name="key">The key for the data</param>
        /// <returns>The data formatted as the correct type</returns>
        public T GetPropertyBagItem<T>(PropertyBagItemTypeEnum key, T defaultValue)
        {
            // Check to see if the item exists in the property bag list
            PropertyBagItem propertyBagItem = propertyBagItems
                .Where(item => item.ItemType.PropertyType == key)
                .FirstOrDefault();

            // Return the value
            return (T)((propertyBagItem == null) ? defaultValue : propertyBagItem.Value);
        }
    }
}
