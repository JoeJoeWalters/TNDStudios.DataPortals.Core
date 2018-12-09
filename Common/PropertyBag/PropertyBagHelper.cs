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
        public T Get<T>(PropertyBagItemTypeEnum key, T defaultValue)
        {
            // Check to see if the item exists in the property bag list
            PropertyBagItem propertyBagItem = propertyBagItems
                .Where(item => item.ItemType.PropertyType == key)
                .FirstOrDefault();

            // Return the value
            return (T)((propertyBagItem == null) ? defaultValue : propertyBagItem.Value);
        }

        /// <summary>
        /// Set the value of the property bag item
        /// </summary>
        /// <typeparam name="T">The type of data being sent</typeparam>
        /// <param name="key">The key of the item to be changed</param>
        /// <param name="value"></param>
        /// <returns>Success or not</returns>
        public Boolean Set<T>(PropertyBagItemTypeEnum key, T value)
        {
            Boolean result = false;

            // Scan the property bag and change all the items that match
            propertyBagItems
                .ForEach(item =>
                {
                    if (item.ItemType.PropertyType == key)
                    {
                        item.Value = value;
                        result = true; // Set as a success
                    }
                });
            
            // Tell the caller if it worked or not
            return result;
        }
    }
}
