using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using CsvHelper;

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
        /// Get the data from a field
        /// </summary>
        /// <param name="csvReader">The reader to handle the property get</param>
        /// <param name="property">The property data</param>
        /// <returns>If it was successful</returns>
        private Boolean GetPropertyValue(CsvReader csvReader, DataItemProperty property, ref Object value)
        {
            // Check to see if it by oridinal reference or by name
            Boolean fieldFound =
                (property.OridinalPosition != -1) ?
                    csvReader.TryGetField(property.DataType, property.OridinalPosition, out value) :
                    csvReader.TryGetField(property.DataType, property.Name, out value);

            // Still empty? Check and see if certain special cases have not happened
            if (!fieldFound)
            {
                // Get the raw data for the field
                Object objectData = "";
                fieldFound = (property.OridinalPosition != -1) ?
                        csvReader.TryGetField(typeof(String), property.OridinalPosition, out objectData) :
                        csvReader.TryGetField(typeof(String), property.Name, out objectData);
                String rawData = fieldFound ? (String)objectData : "";

                // Additional things based on the data type
                switch (property.DataType.ToString().ToLower().Replace("system.", ""))
                {
                    case "double":

                        // Parse the data to a double
                        Double parsedValue = (Double)0;
                        if (Double.TryParse(rawData, out parsedValue))
                            value = parsedValue;

                        break;

                    case "boolean":

                        // Get the first character of the raw data if there is some
                        Char firstChar =
                            (rawData.Length > 0) ? rawData.ToCharArray()[0] : ' ';

                        // Check the first character to see if it matches a true state
                        switch (firstChar)
                        {
                            case '1':
                            case 'y':
                            case 't':
                            case 'f':
                                value = true;
                                break;
                            default:
                                value = false;
                                break;
                        }

                        break;
                }
            }

            // Check again
            fieldFound = (value != null);

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
