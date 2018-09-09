using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using CsvHelper;

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
        public override Boolean ExecuteNonQuery(DataItemDefinition definition, string command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Query the flat file and return multiple records
        /// </summary>
        /// <param name="definition">The definition of the flat file</param>
        /// <param name="command">The command to execute on the definition</param>
        /// <returns>A list of data items that match the query</returns>
        public override DataTable ExecuteReader(DataItemDefinition definition, String command)
        {
            // Create a list of data items to return
            DataTable dataItems = definition.ToDataTable();

            // Open up a text reader to stream the data to the CSV Reader
            using (TextReader textReader = new StringReader(this.fileData ?? ""))
            {
                // Create an instance of the CSV Reader
                using (CsvReader csvReader = new CsvReader(textReader))
                {
                    // Configure the CSV Reader
                    csvReader.Configuration.HasHeaderRecord = false;
                    csvReader.Configuration.BadDataFound = null;

                    // Loop the records
                    while (csvReader.Read())
                    {
                        DataRow dataRow = dataItems.NewRow(); // Create a new row to populate

                        // Match all of the properties in the definitions lists
                        definition.Properties.ForEach(
                            property =>
                            {
                                // Try and get the value
                                Object field = null;
                                Boolean fieldFound = GetPropertyValue(csvReader, property, ref field);

                                // Found something?
                                if (fieldFound)
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
            response = (property.OridinalPosition != -1) ?
                csvReader.TryGetField(overridingDataType, property.OridinalPosition, out tempValue) :
                csvReader.TryGetField(overridingDataType, property.Name, out tempValue);

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
            => (value ?? "")
                .Replace(csvReader.Configuration.Delimiter, "")
                .Replace(csvReader.Configuration.Quote, ' ')
                .Trim();

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

                    break;

                case "datetime":

                    // Check to see if it by oridinal reference or by name
                    fieldFound = GetField<String>(csvReader, property, typeof(String), out String rawDateValue);
                    if (fieldFound)
                    {
                        // Clean the string up for parsing
                        rawDateValue = CleanString(rawDateValue, csvReader);

                        // Try and parse the datetime field
                        DateTime formattedDate = DateTime.ParseExact(rawDateValue, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        if (formattedDate != null)
                            value = formattedDate;
                    }

                    break;

                default:

                    // Get everything else as a string
                    fieldFound = GetField<Object>(csvReader, property, out value);

                    break;            
            }
            
            // Return the data
            return fieldFound;
        }

        /// <summary>
        /// Query the flat file and return a single record
        /// </summary>
        /// <param name="definition">The definition of the flat file</param>
        /// <param name="command">The command to execute on the definition</param>
        /// <returns>A single item that matched the query</returns>
        public override DataTable ExecuteScalar(DataItemDefinition definition, string command)
        {
            throw new NotImplementedException();
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
