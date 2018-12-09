using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using TNDStudios.DataPortals.PropertyBag;

namespace TNDStudios.DataPortals.Data
{
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
        /// The specific culture information for this definition
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// The encoding format for the definition
        /// </summary>
        public Encoding EncodingFormat { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public DataItemDefinition() : base()
        {
            Name = $"Item Definition {Id.ToString()}";
            ItemProperties = new List<DataItemProperty>();
            EncodingFormat = Encoding.Default;
            Culture = CultureInfo.CurrentCulture;
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

                        try
                        {
                            // Try and add the column in
                            result.Columns.Add(
                                new DataColumn(property.Name, property.DataType, property.Calculation)
                                {
                                });
                        }
                        catch
                        {
#warning "Bit of a hack for now, expression columns can reference other expression columns but they need to be added in the right order, fix this later by adding the expressions after the fact"
                            // Cannot create a calculated column based on another calculated column
                            // So add in a column where the calculation is empty
                            result.Columns.Add(
                                new DataColumn(property.Name, property.DataType, "")
                                {
                                });
                        }

                        break;
                }
            });

            return result; // Return the data table
        }
    }
}
