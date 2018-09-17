using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using TNDStudios.DataPortals.Helpers;

namespace TNDStudios.DataPortals.Data
{
    public class FlatFileProvider : DataProviderBase, IDataProvider
    {
        /// <summary>
        /// The data that was derived from the stream given to connect to
        /// if the connection was not via a connection string (file location)
        /// </summary>
        private String fileData;

        /// <summary>
        /// Connect to the flat file source
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <returns>If the file exists when it is connected</returns>
        public override Boolean Connect(String connectionString)
        {
            this.connectionString = connectionString; // Assign the connection string
            this.fileData = ""; // The data if it came from a stream rather than a file on disk

            // Does the file actually exist?
            if (File.Exists(this.connectionString))
            {
                // Read the data in to the file data storage area so it's common when performing queries
                using (TextReader reader = File.OpenText(this.connectionString))
                {
                    this.fileData = reader.ReadToEnd();
                }
            }

            // Indicate that it is connected if it actually got some data
            connected = (File.Exists(this.connectionString));
            return connected;
        }

        /// <summary>
        /// Connect to a stream of flat file data 
        /// </summary>
        /// <param name="stream">The stream of data to connect to</param>
        /// <returns>If the data was valid and is a stream</returns>
        public override Boolean Connect(Stream stream)
        {
            this.connectionString = ""; // Blank out the connection string as we are using a stream instead
            this.fileData = ""; // Blank file data by default

            // Read the data from the stream provided
            using (StreamReader textReader = new StreamReader(stream))
            {
                this.fileData = textReader.ReadToEnd();
            }

            // Connected now? I.E. did it get some data?
            connected = ((this.fileData ?? "") != "");
            return connected;
        }

        /// <summary>
        /// Disconnect (Pseudo-Disconnect) from the file
        /// </summary>
        /// <returns>Always true as the file will not actually be disconnected</returns>
        public override Boolean Disconnect()
        {
            connected = false; // Always disconnect
            fileData = ""; // No file data held once disconnected
            return connected;
        }

        /// <summary>
        /// Execute a non-query command on the flat file
        /// </summary>
        /// <param name="definition">The definition of the flat file</param>
        /// <param name="command">The command to execute on the definition</param>
        /// <returns>If the command executed correctly</returns>
        public override Boolean Write(DataItemDefinition definition, DataTable data, string command)
        {
            // Get the stream from the file
            using (MemoryStream textStream = new MemoryStream())
            {
                // Set up the writer
                StreamWriter streamWriter = new StreamWriter(textStream);
                using (CsvWriter writer = SetupWriter(definition, streamWriter))
                {
                    // Do we need to write a header?
                    if (definition.GetPropertyBagItem<Boolean>(DataItemPropertyBagItem.HasHeaderRecord, false))
                    {
                        // Loop the header records and output the header record line manually
                        foreach (DataItemProperty header in definition.ItemProperties)
                        {
                            writer.WriteField(header.Name);
                        }

                        // Move to the next line and flush the data
                        writer.NextRecord();
                        streamWriter.Flush();
                    }

                    // Loop through the actual records and add them to the csv
                    foreach (DataRow row in data.Rows)
                    {
                        // Loop the header records and output the header record line manually
                        definition.ItemProperties.ForEach(property =>
                            {
                                writer.WriteField(DataFormatHelper.WriteData(row[property.Name], property, definition));
                            });

                        // Move to the next line and flush the data
                        writer.NextRecord();
                        streamWriter.Flush();
                    }

                    // Put the data back in the buffer
                    textStream.Position = 0;
                    this.fileData = (new StreamReader(textStream)).ReadToEnd();

                }
            }

            return true;
        }

        /// <summary>
        /// Query the flat file and return multiple records
        /// </summary>
        /// <param name="definition">The definition of the flat file</param>
        /// <param name="command">The command to execute on the definition</param>
        /// <returns>A list of data items that match the query</returns>
        public override DataTable Read(DataItemDefinition definition, String command)
        {
            // Create a list of data items to return
            DataTable dataItems = definition.ToDataTable();

            // Open up a text reader to stream the data to the CSV Reader
            using (TextReader textReader = new StringReader(this.fileData ?? ""))
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

            // Return the items
            return dataItems;
        }

        /// <summary>
        /// Set up a new csv writer based on the definition given
        /// </summary>
        /// <param name="definition">The data item definition</param>
        /// <returns></returns>
        public CsvWriter SetupWriter(DataItemDefinition definition, TextWriter textWriter)
        {
            // Create the new writer
            CsvWriter writer = new CsvWriter(textWriter);

            // Force all fields to be quoted or not
            writer.Configuration.QuoteAllFields = 
                definition.GetPropertyBagItem<Boolean>(DataItemPropertyBagItem.QuoteAllFields, true); 

            return writer;
        }

        /// <summary>
        /// Create and set up a csv reader based on the data item definition given
        /// </summary>
        /// <param name="textReader">The text reader to inject in to the CSV reader</param>
        /// <param name="definition">The definition of the file</param>
        /// <returns>The newly configured CSV Reader</returns>
        public CsvReader SetupReader(TextReader textReader, DataItemDefinition definition)
        {
            // Produce a new CSV Reader
            CsvReader result = new CsvReader(textReader);

            // Configure the CSV Reader
            result.Configuration.HasHeaderRecord = 
                definition.GetPropertyBagItem<Boolean>(DataItemPropertyBagItem.HasHeaderRecord, true);
            result.Configuration.BadDataFound = null; // Don't pipe bad data
            result.Configuration.CultureInfo = definition.Culture;
            result.Configuration.TrimOptions = TrimOptions.Trim;
            result.Configuration.IgnoreQuotes = 
                definition.GetPropertyBagItem<Boolean>(DataItemPropertyBagItem.IgnoreQuotes, true);

            // Send the reader back
            return result;
        }

        /// <summary>
        /// Get a field of type T from the current csv reader row
        /// </summary>
        /// <typeparam name="T">The response type of the data</typeparam>
        /// <param name="reader">The CSV reader to read the dat from</param>
        /// <param name="dataType">The </param>
        /// <param name="value"></param>
        /// <returns></returns>
        private Boolean GetField<T>(CsvReader csvReader, DataItemProperty property, out T value)
            => GetField<T>(csvReader, property, property.DataType, out value);

        private Boolean GetField<T>(CsvReader csvReader,
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

        /// <summary>
        /// Get the data from a field
        /// </summary>
        /// <param name="csvReader">The reader to handle the property get</param>
        /// <param name="property">The property data</param>
        /// <returns>If it was successful</returns>
        private Boolean GetPropertyValue(CsvReader csvReader, DataItemProperty property, DataItemDefinition definition, ref Object value)
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
        /// Default Constructor
        /// </summary>
        public FlatFileProvider()
        {
            connectionString = ""; // No connection string by default
            connected = false; // Not connected until the file can be proven as existing
        }
    }
}
