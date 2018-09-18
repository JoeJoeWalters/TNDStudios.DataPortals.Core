using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Data;
using System.IO;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.Helpers
{
    public partial class FlatFileHelper
    {
        /// <summary>
        /// Read the raw data file and populate the in-memory data table with it
        /// </summary>
        /// <param name="rawData">The raw flat file data from wherever it came from</param>
        /// <returns>If the translation was successful</returns>
        public static DataTable TextToDataTable(DataItemDefinition definition, String rawData)
        {
            // Create a list of data items to return
            DataTable dataItems = definition.ToDataTable();

            // Raw data has something to convert?
            if ((rawData ?? "") != "")
            {
                // Open up a text reader to stream the data to the CSV Reader
                using (TextReader textReader = new StringReader(rawData))
                {
                    // Create an instance of the CSV Reader
                    using (CsvReader csvReader = SetupReader(textReader, definition))
                    {
                        // Get the record header if needed
                        if (csvReader.Configuration.HasHeaderRecord)
                        {
                            csvReader.Read(); // Do a read first
                            csvReader.ReadHeader();

                            // Parse the header records so that they do not include enclosing quotes
                            Int32 headerId = 0;
                            while (headerId < csvReader.Context.HeaderRecord.Length)
                            {
                                // Clean the header
                                csvReader.Context.HeaderRecord[headerId] =
                                    DataFormatHelper.CleanString(
                                        csvReader.Context.HeaderRecord[headerId],
                                        csvReader.Configuration.Quote);

                                headerId++; // Move to the next header
                            }
                        }

                        // Loop the records
                        while (csvReader.Read())
                        {
                            DataRow dataRow = dataItems.NewRow(); // Create a new row to populate

                            // Match all of the properties in the definitions lists
                            definition.ItemProperties.ForEach(
                                property =>
                                {
                                    // Try and get the value
                                    Object field = null;
                                    Boolean fieldFound = GetPropertyValue(csvReader, property, definition, ref field);

                                    // Found something?
                                    if (fieldFound && field != null)
                                        dataRow[property.Name] = field;
                                });

                            // Add the row to the result data table
                            dataItems.Rows.Add(dataRow);
                        }
                    }
                }
            }

            return dataItems; // Send the datatable back
        }

        /// <summary>
        /// Create and set up a csv reader based on the data item definition given
        /// </summary>
        /// <param name="textReader">The text reader to inject in to the CSV reader</param>
        /// <param name="definition">The definition of the file</param>
        /// <returns>The newly configured CSV Reader</returns>
        private static CsvReader SetupReader(TextReader textReader, DataItemDefinition definition)
        {
            // Produce a new CSV Reader
            CsvReader result = new CsvReader(textReader);

            // Configure the CSV Reader
            result.Configuration.HasHeaderRecord =
                definition.GetPropertyBagItem<Boolean>(DataItemPropertyBagItem.HasHeaderRecord, true);
            result.Configuration.BadDataFound = null; // Don't pipe bad data
            result.Configuration.CultureInfo = definition.Culture;
            result.Configuration.TrimOptions = TrimOptions.Trim;
            result.Configuration.Delimiter =
                definition.GetPropertyBagItem<String>(DataItemPropertyBagItem.DelimiterCharacter, ",");
            result.Configuration.Quote =
                definition.GetPropertyBagItem<Char>(DataItemPropertyBagItem.QuoteCharacter, '"');
            result.Configuration.IgnoreQuotes =
                definition.GetPropertyBagItem<Boolean>(DataItemPropertyBagItem.IgnoreQuotes, true);

            // Send the reader back
            return result;
        }

        /// <summary>
        /// Get the data from a field
        /// </summary>
        /// <param name="csvReader">The reader to handle the property get</param>
        /// <param name="property">The property data</param>
        /// <returns>If it was successful</returns>
        private static Boolean GetPropertyValue(CsvReader csvReader, DataItemProperty property, DataItemDefinition definition, ref Object value)
        {
            // Get the proeprty type as some types of data need handling differently straight away
            String propertyType = property.DataType.ToString().ToLower().Replace("system.", "");

            // Get the raw data
            Boolean fieldFound = GetField<String>(csvReader, property, typeof(String), out String rawValue);
            if (fieldFound)
                value = DataFormatHelper.ReadData(DataFormatHelper.CleanString(rawValue, csvReader.Configuration.Quote), property, definition);

            // Return the data
            return fieldFound;
        }

        /// <summary>
        /// Get a field of type T from the current csv reader row
        /// </summary>
        /// <typeparam name="T">The response type of the data</typeparam>
        /// <param name="reader">The CSV reader to read the dat from</param>
        /// <param name="dataType">The </param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static Boolean GetField<T>(CsvReader csvReader, DataItemProperty property, out T value)
            => GetField<T>(csvReader, property, property.DataType, out value);

        private static Boolean GetField<T>(CsvReader csvReader,
            DataItemProperty property,
            Type overridingDataType,
            out T value)
        {
            Object tempValue = null; // The temporary value before it is cast
            Boolean response = false; // Successful?

            // Try and get the value from either the oridinal position or by the 
            // column name
            Int32 calculatedPosition =
                (property.OridinalPosition != -1) ?
                    property.OridinalPosition :
                    Array.IndexOf(csvReader.Context.HeaderRecord, property.Name);

            response = csvReader.TryGetField(overridingDataType, calculatedPosition, out tempValue);

            // Return the value casted to the required type
            value = (T)tempValue;

            // Return if it was successful
            return response;
        }

    }
}
