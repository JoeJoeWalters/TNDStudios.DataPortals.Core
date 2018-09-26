using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.Helpers
{
    public partial class FlatFileHelper
    {
        /// <summary>
        /// Analyse some raw data and work out how 
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static DataItemDefinition AnalyseText(String rawData)
        {
            // Start with a blank definition
            DataItemDefinition result = new DataItemDefinition() { };

            // Raw data has something to convert?
            if ((rawData ?? "") != "")
            {
                // Open up a text reader to stream the data to the CSV Reader
                using (TextReader textReader = new StringReader(rawData))
                {
                    // Create an instance of the CSV Reader
                    using (CsvReader csvReader = SetupReader(textReader, null))
                    {
                        // Can we read from the stream?
                        Int32 headerId = 0;
                        if (csvReader.Read())
                        {
                            // Read in the "headers", this may not actually be the headers
                            // but is a quick way for us to seperate the amount of columns
                            csvReader.ReadHeader();

                            // Parse the header records so that they do not include enclosing quotes
                            headerId = 0;
                            while (headerId < csvReader.Context.HeaderRecord.Length)
                            {
                                // Clean the header
                                csvReader.Context.HeaderRecord[headerId] =
                                    DataFormatHelper.CleanString(
                                        csvReader.Context.HeaderRecord[headerId],
                                        csvReader.Configuration.Quote);

                                // Add a new property to the definition
                                result.ItemProperties.Add(new DataItemProperty()
                                {
                                    Calculation = "",
                                    DataType = typeof(String),
                                    Description = $"Column {headerId.ToString()}",
                                    Name = csvReader.Context.HeaderRecord[headerId],
                                    Key = false,
                                    OridinalPosition = headerId,
                                    Path = csvReader.Context.HeaderRecord[headerId],
                                    Pattern = "",
                                    PropertyType = DataItemPropertyType.Property
                                });

                                headerId++; // Move to the next header
                            }
                        }

                        // Is there an additional line to calculate the data types
                        // and correct the types to what is found in these columns
                        if (csvReader.Read())
                        {
                            // For each of the properties that we found
                            result.ItemProperties.ForEach(property =>
                            {
                                // Try and get the raw value for this column
                                if (GetField<String>(csvReader,
                                    property,
                                    out String rawValue))
                                {
                                    // Deriver the data type
                                    property.DataType =
                                        DataFormatHelper.CalculateType(
                                            DataFormatHelper.CleanString(
                                                rawValue,
                                                csvReader.Configuration.Quote));
                                };
                            });
                        }
                    }
                }
            }

            return result; // Send the definition back
        }

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
                            definition.ItemProperties
                                .Where(prop => prop.PropertyType == DataItemPropertyType.Property)
                                .ToList()
                                .ForEach(
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
            result.Configuration.HasHeaderRecord = (definition == null) ? true :
                definition.GetPropertyBagItem<Boolean>(DataItemPropertyBagItem.HasHeaderRecord, true);
            result.Configuration.BadDataFound = null; // Don't pipe bad data
            result.Configuration.CultureInfo = (definition == null) ?
                System.Globalization.CultureInfo.CurrentCulture : definition.Culture;
            result.Configuration.TrimOptions = TrimOptions.Trim;
            result.Configuration.Delimiter = (definition == null) ? "," :
                definition.GetPropertyBagItem<String>(DataItemPropertyBagItem.DelimiterCharacter, ",");
            result.Configuration.Quote = (definition == null) ? '"' :
                definition.GetPropertyBagItem<Char>(DataItemPropertyBagItem.QuoteCharacter, '"');
            result.Configuration.IgnoreQuotes = (definition == null) ? true :
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
            Object tempValue = DBNull.Value; // The temporary value before it is cast
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
