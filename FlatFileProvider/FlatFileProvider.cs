﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using CsvHelper;
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

            // Do we have a stream and a definition
            if (stream != null && definition != null)
            {
                stream.Position = 0; // Reset back to the start again in case someone else has read it

                // Read the data from the stream provided
                using (StreamReader textReader = new StreamReader(stream))
                {
                    this.memoryData = FlatFileHelper.TextToDataTable(definition, textReader.ReadToEnd());

                    return true; // Connected without any errors
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

                // Get a list of "primary key" (can be multiple columns that
                // identify the unique records)
                List<String> keys =
                    definition.ItemProperties
                        .Where(prop => prop.Key)
                        .Select(prop => prop.Name)
                        .ToList();

                // Items that did not find a match and need to be added instead
                DataTable rowsToAdd = memoryData.Clone();

                // Find any records that match the primary key values
                // and attempt to update the records, DataTables don't 
                // currently implement IEnumerable so can't use Linq directly
                foreach (DataRow writeRow in data.Rows)
                {
                    Boolean match = false; // Not a match by default
                    Int32 rowNumber = 0; // The row number currently
                    while (!match && rowNumber < memoryData.Rows.Count)
                    {
                        DataRow row = memoryData.Rows[rowNumber]; // Get the current row

                        // Set the match to true, unless proven otherwise
                        match = true;
                        keys.ForEach(key =>
                        {
                            if (!row[key].Equals(writeRow[key])) { match = false; }
                        });

                        // Is this a matching field? If so then update the field
                        if (match)
                            row.ItemArray = writeRow.ItemArray;

                        // Move to the next row
                        rowNumber++;
                    }

                    // Not a match so this will become a row to add
                    if (!match)
                        rowsToAdd.Rows.Add(writeRow.ItemArray);
                }

                // Add the remaining records to the table
                foreach (DataRow row in rowsToAdd.Rows)
                {
                    memoryData.Rows.Add(row.ItemArray); // Add the row
                }

                // Destroy the temporary table
                rowsToAdd.Clear();
                rowsToAdd = null;

                // This worked
                result = true;
            }
            catch { }

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
            // Do we have a filter?
            if ((command ?? "") != "")
            {
                // Create a new view to filter
                DataView view = new DataView(memoryData)
                {
                    RowFilter = command // Set the filter from the command
                }; 

                return view.ToTable(); // Return the filtered table
            }
            else
                return memoryData; // Simply supply the in-memory datatable back to the user
        }
        
        /// <summary>
        /// Look at the file and try and represent the file as a dataset without a definition
        /// </summary>
        /// <returns>A representation of the data</returns>
        public override DataItemDefinition Analyse(AnalyseRequest<Object> request)
        {
            // Create a blank result data table
            DataItemDefinition result = new DataItemDefinition() { };

            String rawData = "";
            switch (request.Data.GetType().ToString().Replace("System.", ""))
            {
                case "String":

                    rawData = (String)request.Data;

                    break;

                default:

                    ((Stream)request.Data).Position = 0; // Reset the stream position

                    // Read the data from the stream
                    StreamReader reader = new StreamReader((Stream)request.Data);
                    rawData = reader.ReadToEnd();
                    
                    // Reset the position again so that it can be re-used
                    ((Stream)request.Data).Position = 0;

                    break;
            }

            // Pass down to the analyse text core function
            AnalyseRequest<String> analyseTextRequest = 
                new AnalyseRequest<String>()
                {
                    Data = rawData
                };
            result = FlatFileHelper.AnalyseText(analyseTextRequest);

            // Send the analysis data table back
            return result;
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
