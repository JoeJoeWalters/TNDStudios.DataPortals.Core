using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// Enumeration to make the property bag values 
    /// standard across all providers
    /// </summary>
    public enum DataItemPropertyBagItem : Int32
    {
        HasHeaderRecord = 0,
        QuoteAllFields = 1,
        IgnoreQuotes = 2,
        QuoteCharacter = 3,
        DelimiterCharacter = 4
    }

    /// <summary>
    /// The definition of a data item (How the properties of
    /// the data item are mapped etc.)
    /// </summary>
    public class DataItemDefinition : CommonObject
    {
        /// <summary>
        /// The list of properties that define the data item
        /// </summary>
        public List<DataItemProperty> ItemProperties { get; set; }

        /// <summary>
        /// Adhoc configuration items for different providers
        /// </summary>
        public Dictionary<String, Object> PropertyBag { get; set; }

        /// <summary>
        /// The specific culture information for this definition
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// The encoding format for the definition
        /// </summary>
        public Encoding EncodingFormat { get; set; }

        /// <summary>
        /// The connections that this definition is assigned to
        /// </summary>
        public List<Guid> Connections { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public DataItemDefinition() : base()
        {
            Name = $"Item Definition {Id.ToString()}";
            ItemProperties = new List<DataItemProperty>();
            PropertyBag = new Dictionary<String, Object>();
            EncodingFormat = Encoding.Default;
            Culture = CultureInfo.CurrentCulture;
            Connections = new List<Guid>(); // The connections that this definition is assigned to
        }

        /// <summary>
        /// Convert this definition to a data table so it can be populated
        /// </summary>
        /// <returns></returns>
        public DataTable ToDataTable()
        {
            DataTable result = new DataTable(); // Build a results table to send back

            // Loop the items in the definition and add them to the column definition
            ItemProperties.ForEach(property =>
            {
                switch (property.PropertyType)
                {
                    case DataItemPropertyType.Property:

                        result.Columns.Add(
                            new DataColumn(property.Name, property.DataType)
                            {
                            });

                        break;

                    case DataItemPropertyType.Calculated:

                        result.Columns.Add(
                            new DataColumn(property.Name, property.DataType, property.Calculation)
                            {
                            });

                        break;
                }
            });

            return result; // Return the data table
        }

        /// <summary>
        /// Get a configuration item from the property bag
        /// and format it as appropriate
        /// </summary>
        /// <typeparam name="T">The type of data that is requested</typeparam>
        /// <param name="key">The key for the data</param>
        /// <returns>The data formatted as the correct type</returns>
        public T GetPropertyBagItem<T>(DataItemPropertyBagItem key, T defaultValue)
            => GetPropertyBagItem<T>(key.ToString(), defaultValue);

        public T GetPropertyBagItem<T>(String key, T defaultValue)
            => PropertyBag.ContainsKey(key) ? (T)PropertyBag[key] : defaultValue;


    }
}
