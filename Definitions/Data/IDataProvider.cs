using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TNDStudios.DataPortals.Core.Data
{
    /// <summary>
    /// Interface to define how a data provider should behave
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// The connection string for the data provider
        /// e.g. SQL connection string or path for a file etc.
        /// </summary>
        String ConnectionString { get; }

        /// <summary>
        /// Connect to the location of the data
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <returns></returns>
        Boolean Connect(String connectionString);

        /// <summary>
        /// If the provider is connected
        /// </summary>
        Boolean Connected { get; }

        /// <summary>
        /// Disconnect from the data source
        /// </summary>
        /// <returns>Confirmation of if it has disconnected</returns>
        Boolean Disconnect();

        /// <summary>
        /// Retrieve a set of data based on the query passed to the source
        /// </summary>
        /// <param name="definition">The data defintion of the source of the data</param>
        /// <param name="command">The command to execute to retrieve the data</param>
        /// <returns>A list of data items that the reader found</returns>
        DataTable ExecuteReader(DataItemDefinition definition, String command);

        /// <summary>
        /// Retrieve a single data item based on the query passed to the source
        /// </summary>
        /// <param name="definition">The data defintion of the source of the data</param>
        /// <param name="command">The command to execute to retrieve the data</param>
        /// <returns>A single data item that was found</returns>
        DataTable ExecuteScalar(DataItemDefinition definition, String command);

        /// <summary>
        /// Execute a query on the data source without expecting data back
        /// </summary>
        /// <param name="definition">The data defintion of the source of the data</param>
        /// <param name="command">The command to execute to manipulate the data</param>
        /// <returns>If the command was successful</returns>
        Boolean ExecuteNonQuery(DataItemDefinition definition, String command);
    }
}
