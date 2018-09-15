using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace TNDStudios.DataPortals.Data
{
    public class FlatFileProvider : DataProviderBase, IDataProvider
    {
        /// <summary>
        /// Constants that identify the character of a string as being 
        /// a positive boolean value
        /// </summary>
        private const String booleanChars = "1ytf";

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
                    if (definition.GetPropertyBagItem<Boolean>("HasHeaderRecord", false))
                    {
                        // Loop the header records and output the header record line manually
                        foreach (DataItemProperty header in definition.ItemProperties)
                        {
                            writer.WriteField(FormatWriteHeader(writer, header));
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
                                writer.WriteField(FormatWriteField(writer, property, row[property.Name]));
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
        /// Format the header field so that it is properly quoted / formatted
        /// </summary>
        /// <param name="writer">The Csv Writer used in the write operation</param>
        /// <param name="property">The definition of how items are to be written to the stream</param>
        /// <param name="header">The "name" of the header (the text)</param>
        /// <returns></returns>
        private String FormatWriteHeader(CsvWriter writer, DataItemProperty property)
        {
            // Return the formatted header
            return (property.Name ?? "");
        }

        /// <summary>
        /// Format the field data in to the correct format
        /// </summary>
        /// <param name="property">The property definition for the field</param>
        /// <param name="field">The raw data to be formatted for writing</param>
        /// <returns>The field in the correct format</returns>
        private String FormatWriteField(CsvWriter writer, DataItemProperty property, Object field)
        {
            // Return the formatted field
            return field.ToString();
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
                                CleanString(csvReader.Context.HeaderRecord[headerId], csvReader);

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
                                Boolean fieldFound = GetPropertyValue(csvReader, property, ref field);

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
                definition.GetPropertyBagItem<Boolean>("QuoteAllFields", true); 

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
                definition.GetPropertyBagItem<Boolean>("HasHeaderRecord", true);
            result.Configuration.BadDataFound = null; // Don't pipe bad data
            result.Configuration.CultureInfo = definition.Culture;
            result.Configuration.TrimOptions = TrimOptions.Trim;
            result.Configuration.IgnoreQuotes = 
                definition.GetPropertyBagItem<Boolean>("IgnoreQuotes", true);

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
        /// Clean out a piece of data so it can be handled manually without possible quotes etc.
        /// </summary>
        /// <param name="value">The raw value</param>
        /// <param name="csvReader">The reader that holds the configuration</param>
        /// <returns></returns>
        private String CleanString(String value, CsvReader csvReader)
            => RemoveEnds(value, csvReader.Configuration.Quote).Trim();

        /// <summary>
        /// Remove the character from the start and/or end of the string
        /// but not in the middle
        /// </summary>
        /// <param name="value"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        private String RemoveEnds(String value, Char character)
        {
            // Split up in to an array of characters
            value = (value ?? "").Trim(); // Handle any incoming nulls
            if (value.Length >= 2)
            {
                if (value.StartsWith(character)) { value = value.Remove(0, 1); }
                if (value.EndsWith(character)) { value = value.Remove(value.Length - 1, 1); }
            }

            return value; // Return the formatted string
        }

        /// <summary>
        /// Check to see if a string is numeric (to wrap custom handlers)
        /// </summary>
        /// <param name="value">The string to check against</param>
        /// <returns>If the string is numeric or not</returns>
        private Boolean IsNumeric(String value) =>
            int.TryParse(value, out int chuckInt) ||
            float.TryParse(value, out float chuckFloat);

        /// <summary>
        /// Get the data from a field
        /// </summary>
        /// <param name="csvReader">The reader to handle the property get</param>
        /// <param name="property">The property data</param>
        /// <returns>If it was successful</returns>
        private Boolean GetPropertyValue(CsvReader csvReader, DataItemProperty property, ref Object value)
        {
            // Get the proeprty type as some types of data need handling differently straight away
            String propertyType = property.DataType.ToString().ToLower().Replace("system.", "");
            Boolean fieldFound = false; // Field not found by default

            // Check the property type
            switch (propertyType)
            {
                case "boolean":
                case "bool":

                    // Check to see if it by oridinal reference or by name
                    fieldFound = GetField<String>(csvReader, property, typeof(String), out String rawBooleanValue);
                    if (fieldFound)
                    {
                        // Clean the string up for parsing
                        rawBooleanValue = CleanString(rawBooleanValue, csvReader);

                        // Get the first character of the raw data if there is some
                        Char firstChar =
                            (rawBooleanValue.Length > 0) ? rawBooleanValue.ToCharArray()[0] : ' ';

                        // Check the first character to see if it matches a true state
                        value = booleanChars.Contains(firstChar);
                    }

                    break;

                case "double":
                case "int":
                case "unint":
                case "int16":
                case "int32":
                case "int64":
                case "float":
                case "decimal":
                case "single":
                case "byte":
                case "sbyte":
                case "short":
                case "ushort":
                case "long":
                case "ulong":

                    // Check to see if it by oridinal reference or by name
                    fieldFound = GetField<String>(csvReader, property, typeof(String), out String rawNumericValue);
                    if (fieldFound)
                    {
                        // Clean the string up for parsing
                        rawNumericValue = CleanString(rawNumericValue, csvReader);
                        if (IsNumeric(rawNumericValue))
                            value = rawNumericValue;
                    }

                    break;

                case "datetime":

                    // Check to see if it by oridinal reference or by name
                    fieldFound = GetField<String>(csvReader, property, typeof(String), out String rawDateValue);
                    if (fieldFound)
                    {
                        // Clean the string up for parsing
                        rawDateValue = CleanString(rawDateValue, csvReader);

                        // Try and parse the datetime field
                        try
                        {
                            // Make sure there reall is some data
                            if ((rawDateValue ?? "") != "")
                            {
                                // If a specific pattern has been provided then use that, otherwise use the culuture information provided
                                DateTime formattedDate;

                                // Do we have a manual property pattern or just leave it to the culture?
                                if ((property.Pattern ?? "") != "")
                                    formattedDate = DateTime.ParseExact(rawDateValue, (property.Pattern ?? ""), CultureInfo.InvariantCulture);
                                else
                                    formattedDate = DateTime.Parse(rawDateValue, csvReader.Configuration.CultureInfo);

                                // Anything found? If so set it
                                if (formattedDate != null)
                                    value = formattedDate;
                            }
                        }
                        catch
                        {
                            value = null;
                        }
                    }

                    break;

                default:

                    // Get everything else as a string and try and fit it using the 
                    // standard in-built converters
                    fieldFound = GetField<Object>(csvReader, property, out value);
                    if (fieldFound)
                    {
                        value = CleanString((String)value, csvReader);
                    };

                    break;
            }

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
