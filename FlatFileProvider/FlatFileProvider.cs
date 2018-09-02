using System;
using System.Collections.Generic;
using System.IO;

namespace TNDStudios.DataPortals.Core.Data
{
    public class FlatFileProvider : IDataProvider
    {
        /// <summary>
        /// Provide a read-only view of the connection string
        /// </summary>
        private String connectionString;
        public String ConnectionString => connectionString;

        /// <summary>
        /// Is the flat file provider connected to it's source?
        /// </summary>
        private Boolean connected;
        public Boolean Connected => connected;

        /// <summary>
        /// Connect to the flat file source
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <returns>If the file exists when it is connected</returns>
        public Boolean Connect(String connectionString)
        {
            this.connectionString = connectionString; // Assign the connection string
            connected = (File.Exists(this.connectionString)); // Does the file exist? Therefor it is connected
            return connected;
        }

        /// <summary>
        /// Disconnect (Pseudo-Disconnect) from the file
        /// </summary>
        /// <returns>Always true as the file will not actually be disconnected</returns>
        public Boolean Disconnect()
        { 
            connected = false; // Always disconnect
            return connected;
        }

        /// <summary>
        /// Execute a non-query command on the flat file
        /// </summary>
        /// <param name="definition">The definition of the flat file</param>
        /// <param name="command">The command to execute on the definition</param>
        /// <returns>If the command executed correctly</returns>
        public Boolean ExecuteNonQuery(DataItemDefinition definition, string command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Query the flat file and return multiple records
        /// </summary>
        /// <param name="definition">The definition of the flat file</param>
        /// <param name="command">The command to execute on the definition</param>
        /// <returns>A list of data items that match the query</returns>
        public IEnumerable<DataItem> ExecuteReader(DataItemDefinition definition, String command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Query the flat file and return a single record
        /// </summary>
        /// <param name="definition">The definition of the flat file</param>
        /// <param name="command">The command to execute on the definition</param>
        /// <returns>A single item that matched the query</returns>
        public DataItem ExecuteScalar(DataItemDefinition definition, string command)
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
