using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// The definition of a data item (How the properties of
    /// the data item are mapped etc.)
    /// </summary>
    public class DataItemDefinition
    {
        /// <summary>
        /// The list of properties that define the data item
        /// </summary>
        public List<DataItemProperty> Properties { get; set; }

        /// <summary>
        /// The specific culture information for this definition
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// If there are headers to the data source?
        /// </summary>
        public Boolean HasHeaderRecord { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public DataItemDefinition() =>
            Initialise(new List<DataItemProperty>());

        /// <summary>
        /// Constructor with the property list being passed in
        /// </summary>
        /// <param name="properties">The initial list of properties</param>
        public DataItemDefinition(List<DataItemProperty> properties) =>
            Initialise(properties);

        /// <summary>
        /// Initialise the data definition
        /// </summary>
        /// <param name="properties"></param>
        public void Initialise(List<DataItemProperty> properties)
        {
            Properties = properties; // Set the data properties of the definition
            Culture = CultureInfo.CurrentCulture; // Set the culture information to the current by default
            HasHeaderRecord = false; // By default assume there are no headers (I.e. skip the first line)
        }

        /// <summary>
        /// Convert this definition to a data table so it can be populated
        /// </summary>
        /// <returns></returns>
        public DataTable ToDataTable()
        {
            DataTable result = new DataTable(); // Build a results table to send back

            // Loop the items in the definition and add them to the column definition
            Properties.ForEach(property => 
            {
                result.Columns.Add(
                    new DataColumn(property.Name, property.DataType)
                    {                         
                    });
            });

            return result; // Return the data table
        }

    }
}
