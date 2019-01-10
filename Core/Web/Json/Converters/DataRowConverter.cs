using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TNDStudios.DataPortals.Web.Json
{
    /// <summary>
    /// Converts a <see cref="DataRow"/> object to and from JSON.
    /// </summary>
    public class DataRowConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        public override void WriteJson(JsonWriter writer, object dataRow, JsonSerializer jsonSerializer)
        {
            // Cast the incoming object signature as a proper data row to avoid multiple casts
            DataRow row = dataRow as DataRow;

            // Start the tags to open an object
            writer.WriteStartObject();

            // Loop the columns in the row
            foreach (DataColumn column in row.Table.Columns)
            {
                // Check and see if we have an alias for the name of the column
                // in the extended properties, if so then render this as the column
                // name instead
                String columnName = (column.ExtendedProperties.ContainsKey("Alias") && (column.ExtendedProperties["Alias"].ToString() ?? String.Empty) != String.Empty) ? 
                    column.ExtendedProperties["Alias"].ToString() : 
                    column.ColumnName;

                // Write the name of the column (or alias)
                writer.WritePropertyName(columnName);

                // Write out the object itself associated with the column
                jsonSerializer.Serialize(writer, row[column]);
            }

            // Write the tags to close and object
            writer.WriteEndObject();
        }

        /// <summary>
        /// Determines whether this instance can convert the specified value type.
        /// </summary>
        /// <param name="valueType">Type of the value.</param>
        /// <returns>
        ///     <c>true</c> if this instance can convert the specified value type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type valueType)
        {
            return typeof(DataRow).IsAssignableFrom(valueType);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer jsonSerializer)
        {
            throw new NotImplementedException();
        }
    }
}
