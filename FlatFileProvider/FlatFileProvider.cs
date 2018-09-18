using System;
using System.Data;
using System.IO;
using TNDStudios.DataPortals.Helpers;

namespace TNDStudios.DataPortals.Data
{
    public class FlatFileProvider : DataProviderBase, IDataProvider
    {
        /// <summary>
        /// The data that is loaded from the file when the refresh of the
        /// file is made to avoid needing to lock a file (Commiting the data
        /// will overwrite the file with the new data)
        /// </summary>
        private DataTable memoryData;

        /// <summary>
        /// The definition of how the data should be structured
        /// </summary>
        private DataItemDefinition definition;

        /// <summary>
        /// Connect to the flat file source
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <returns>If the file exists when it is connected</returns>
        public override Boolean Connect(DataItemDefinition definition, String connectionString)
        {
            Boolean result = false; // Failed by default

            // Does the file that we are trying to connet to exist?
            if (File.Exists(connectionString))
            {
                // Connect to the file and read the data from it
                using (Stream fileStream = File.OpenRead(connectionString))
                {
                    result = Connect(definition, fileStream); // Do a standard stream connect to reuse that code
                    if (result)
                        this.connectionString = connectionString; // Remember the connection string
                }
            }

            return result; // Return the result of the read
        }

        /// <summary>
        /// Connect to a stream of flat file data 
        /// </summary>
        /// <param name="stream">The stream of data to connect to</param>
        /// <returns>If the data was valid and is a stream</returns>
        public override Boolean Connect(DataItemDefinition definition, Stream stream)
        {
            this.connectionString = ""; // Blank out the connection string as we are using a stream instead
            this.definition = definition; // Assign the definition to use
            this.memoryData = new DataTable(); // Blank data by default

            if (stream != null)
            {
                // Read the data from the stream provided
                using (StreamReader textReader = new StreamReader(stream))
                {
                    this.memoryData = FlatFileHelper.TextToDataTable(definition, textReader.ReadToEnd());
                }
            }

            return false; // Failed if we get to here
        }

        /// <summary>
        /// Disconnect (Pseudo-Disconnect) from the file
        /// </summary>
        /// <returns>Always true as the file will not actually be disconnected</returns>
        public override Boolean Disconnect()
        {
            connected = false; // Always disconnect
            memoryData = new DataTable(); // No data held once disconnected
            return connected;
        }

        /// <summary>
        /// Execute a write on the in-memory data-table
        /// </summary>
        /// <param name="definition">The definition of the flat file</param>
        /// <param name="command">The command to execute on the definition</param>
        /// <returns>If the command executed correctly</returns>
        public override Boolean Write(DataTable data, string command)
        {
            Boolean result = false;
            try
            {
                memoryData.Merge(data);
                result = true;
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Commit the data back to the file
        /// </summary>
        /// <returns>If the write was successful</returns>
        public override Boolean Commit()
        {
            Boolean result = false; // Failed by default

            // Generate the flat file content based on the definition when connecting
            String flatFileContent = FlatFileHelper.DataTableToString(this.definition, this.memoryData);

            // Try and write the file to disk
            try
            {
                // Write the file
                File.WriteAllText(this.connectionString, flatFileContent, definition.EncodingFormat);

                // Does the file now exist (and there were no errors writing)
                result = File.Exists(this.connectionString);
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Query the flat file and return multiple records
        /// </summary>
        /// <param name="command">The command to execute on the definition</param>
        /// <returns>A list of data items that match the query</returns>
        public override DataTable Read(String command)
        {
            return memoryData; // Simply supply the in-memory datatable back to the user
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
