using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using TNDStudios.DataPortals.Helpers;
using TNDStudios.DataPortals.PropertyBag;

namespace TNDStudios.DataPortals.Data
{
    public class DelimitedFileProvider : DataProviderBase, IDataProvider
    {
        /// <summary>
        /// Override to the base behaviour and allow data writing
        /// </summary>
        public override Boolean CanWrite { get => true; }

        /// <summary>
        /// Override to the base behaviour and the provider to analyse
        /// </summary>
        public override Boolean CanAnalyse { get => true; }

        /// <summary>
        /// The property bag types that can be used to define this connection
        /// </summary>
        public override List<PropertyBagItemType> PropertyBagTypes() => 
            new List<PropertyBagItemType>()
            {
                new PropertyBagItemType(){ DataType = typeof(String), DefaultValue = ",", PropertyType = PropertyBagItemTypeEnum.DelimiterCharacter },
                new PropertyBagItemType(){ DataType = typeof(Boolean), DefaultValue = true, PropertyType = PropertyBagItemTypeEnum.HasHeaderRecord },
                new PropertyBagItemType(){ DataType = typeof(Boolean), DefaultValue = true, PropertyType = PropertyBagItemTypeEnum.IgnoreQuotes },
                new PropertyBagItemType(){ DataType = typeof(Boolean), DefaultValue = true, PropertyType = PropertyBagItemTypeEnum.QuoteAllFields },
                new PropertyBagItemType(){ DataType = typeof(Char), DefaultValue = '"', PropertyType = PropertyBagItemTypeEnum.QuoteCharacter },
                new PropertyBagItemType(){ DataType = typeof(Int32), DefaultValue = 0, PropertyType = PropertyBagItemTypeEnum.RowsToSkip }
            };

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
        public override Boolean Connect(DataItemDefinition definition, DataConnection connection)
        {
            Boolean result = false; // Failed by default

            // Check if not an object
            if (connection == null)
                return false;

            // Get the processed connection string (with any injected items)
            String connectionString = connection.ConnectionStringProcessed;

            // Does the file that we are trying to connect to exist?
            if (File.Exists(connectionString))
            {
                // Connect to the file and read the data from it
                using (Stream fileStream = File.OpenRead(connectionString))
                {
                    result = Connect(definition, connection, fileStream); // Do a standard stream connect to reuse that code
                    if (result)
                        this.Connection = connection; // Remember the connection string
                }
            }

            this.MarkLastAction(); // Tell the provider base class that it did something
            return result; // Return the result of the read
        }

        /// <summary>
        /// Connect to a stream of flat file data 
        /// </summary>
        /// <param name="stream">The stream of data to connect to</param>
        /// <returns>If the data was valid and is a stream</returns>
        public override Boolean Connect(DataItemDefinition definition, DataConnection connection, Stream stream)
        {
            this.Connection = connection;
            this.definition = definition; // Assign the definition to use
            this.memoryData = new DataTable(); // Blank data by default

            // Do we have a stream and a definition
            if (stream != null && definition == null)
            {
                base.connected = true;
                return true;
            }
            else if (stream != null && definition != null)
            {
                stream.Position = 0; // Reset back to the start again in case someone else has read it
                base.MarkLastAction(); // Tell the provider base class that it did something

                // Read the data from the stream provided
                using (StreamReader textReader = new StreamReader(stream))
                {
                    this.memoryData = DelimitedFileHelper.TextToDataTable(definition, connection, textReader.ReadToEnd());
                    base.connected = true; // Mark the provider as connected
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
            base.MarkLastAction(); // Tell the provider base class that it did something
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
                base.MarkLastAction(); // Tell the provider base class that it did something

                // Get a list of "primary key" (can be multiple columns that
                // identify the unique records)
                List<String> keys =
                    definition.ItemProperties
                        .Where(prop => prop.Key != DataItemKeyType.None)
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
            String flatFileContent = DelimitedFileHelper.DataTableToString(this.definition, this.Connection, this.memoryData);

            // Try and write the file to disk
            try
            {
                // Get the processed connection string (with any injected items)
                String connectionString = this.Connection.ConnectionStringProcessed; 

                // Write the file
                File.WriteAllText(connectionString, flatFileContent, definition.EncodingFormat);

                // Does the file now exist (and there were no errors writing)
                result = File.Exists(connectionString);
            }
            catch
            {
            }

            base.MarkLastAction(); // Tell the provider base class that it did something

            // Return the result
            return result;
        }

        /// <summary>
        /// Query the flat file and return multiple records
        /// </summary>
        /// <param name="command">The command to execute on the definition</param>
        /// <returns>A list of data items that match the query</returns>
        public override DataTable Read(String command)
        {
            base.MarkLastAction(); // Tell the provider base class that it did something

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
            String rawData = ""; // The data which ultimately will be read from

            // Check to see what can of analysis is being requested
            if (request.Connection != null &&
                (request.Connection.ConnectionString ?? String.Empty) != String.Empty)
            {
                // Get the stream of data from the raw file
                rawData = File.ReadAllText(request.Connection.ConnectionStringProcessed);
            }
            else
            {
                // No connection was provided so use the raw data provided instead
                switch (request.Data.GetType().ToShortName())
                {
                    case "string":

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
            }

            // Pass down to the analyse text core function
            AnalyseRequest<String> analyseTextRequest =
                new AnalyseRequest<String>()
                {
                    Data = rawData,
                    Connection = request.Connection
                };
            result = DelimitedFileHelper.AnalyseText(analyseTextRequest);

            base.MarkLastAction(); // Tell the provider base class that it did something

            // Send the analysis data table back
            return result;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DelimitedFileProvider() : base()
        {
            Connection = null; // No connection string by default
            connected = false; // Not connected until the file can be proven as existing
        }
    }
}
