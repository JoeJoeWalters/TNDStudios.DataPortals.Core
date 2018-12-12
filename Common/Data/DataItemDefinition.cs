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

                        // Create the new column
                        DataColumn newColumn = new DataColumn(property.Name, property.DataType)
                        {
                        };

                        // Add the column to the array
                        result.Columns.Add(newColumn);

                        // If this is a key column, add it to the table primary keys list
                        if (property.Key)
                            result.PrimaryKey = result.PrimaryKey.Append(newColumn).ToArray(); // Append to the end

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

        /// <summary>
        /// Populate this definition from a given data table
        /// </summary>
        /// <param name="dataTable">The data table to load the definition from</param>
        /// <returns>Success or Failure flag</returns>
        public Boolean FromDataTable(DataTable dataTable)
        {
            // Clear out the item properties
            this.ItemProperties = new List<DataItemProperty>();

            // Loop the columns in the data table and assign new item properties
            // for each of them
            foreach (DataColumn column in dataTable.Columns)
            {
                // Create the new item property
                DataItemProperty itemProperty = new DataItemProperty()
                {
                    Calculation = (column.Expression ?? String.Empty),
                    DataType = column.DataType,
                    Description = column.ColumnName,
                    Id = Guid.NewGuid(),
                    Key = dataTable.PrimaryKey.Contains<DataColumn>(column),
                    Name = column.ColumnName,
                    OrdinalPosition = column.Ordinal,
                    Path = column.ColumnName,
                    PropertyType =
                        (column.Expression ?? String.Empty) != "" ?
                        DataItemPropertyType.Calculated :
                        DataItemPropertyType.Property,
                    Quoted = false,
                    Size = column.MaxLength
                };

                // Add the property to the property array
                this.ItemProperties.Add(itemProperty);
            }

            // Assign any culture info etc. that it can glean
            this.Culture = dataTable.Locale;
            this.EncodingFormat = Encoding.UTF8; // Default

            return true;
        }
    }
}
