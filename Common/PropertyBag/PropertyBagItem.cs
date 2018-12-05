using System;
using System.ComponentModel;

namespace TNDStudios.DataPortals.PropertyBag
{
    /// <summary>
    /// Enumeration to make the property bag values 
    /// standard across all providers
    /// </summary>
    public enum PropertyBagItemTypeEnum : Int32
    {
        /*
        Default Property bag item
        */

        [Description("Unknown Property Bag Element")]
        Unknown = 0,

        /*
        Connection based property bag items
        */

        [Description("Has Header Record")]
        HasHeaderRecord = 1, // Does this definition have a header record in the file etc.

        [Description("Quote All Fields")]
        QuoteAllFields = 2, // Do we quote all of the fields if this is a file provider type of a given type

        [Description("Ignore Quotes")]
        IgnoreQuotes = 3, // Do we ignore quotes in the data?

        [Description("Quote Character")]
        QuoteCharacter = 4, // What is the quote (encapsulating) character (depeing on the provider type)

        [Description("Delimiter Character")]
        DelimiterCharacter = 5, // What is the delimiting character (depending on the provider type)

        /*
        Data Definition based property bag items
        */

        [Description("Rows To Skip")]
        RowsToSkip = 10001 // How many rows to skip (not including the header, depending on the provider type)
    }

    /// <summary>
    /// Model for sending and receiving the data for a property bag item
    /// </summary>
    public class PropertyBagItem
    {
        /// <summary>
        /// The type of property bag item
        /// </summary
        public PropertyBagItemType ItemType { get; set; }

        /// <summary>
        /// The value for this property bag item
        /// </summary>
        public Object Value { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PropertyBagItem()
        {
            Value = null; // No value by default
            ItemType = new PropertyBagItemType(); // Default for the item type
        }
    }
}
