using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using TNDStudios.DataPortals.Helpers;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// Enumeration of all the different registered data providers
    /// that can be used instead of just "typeof"
    /// </summary>
    public enum DataProviderType : Int32
    {
        [Description("")]
        Unknown = 0,

        [Description("Flat File Provider")]
        FlatFileProvider = 1,

        [Description("SQL Server")]
        MSSQLProvider = 2
    }

    /// <summary>
    /// Interface to define how a data provider should behave
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Is the data provider marked as being in test mode
        /// </summary>
        Boolean TestMode { get; set; }

        /// <summary>
        /// The connection string for the data provider
        /// e.g. SQL connection string or path for a file etc.
        /// </summary>
        String ConnectionString { get; }

        /// <summary>
        /// Connect to the location of the data (Database connection, flat file, xml, etc.)
        /// </summary>
        /// <param name="definition">The data defintion of the source of the data</param>
        /// <param name="connectionString">The connection string</param>
        /// <returns></returns>
        Boolean Connect(DataItemDefinition definition, String connectionString);

        /// <summary>
        /// Connect to a given stream of data (could be a flat file, xml etc.)
        /// </summary>
        /// <param name="definition">The data defintion of the source of the data</param>
        /// <param name="stream">The stream of data to process</param>
        /// <returns></returns>
        Boolean Connect(DataItemDefinition definition, Stream stream);

        /// <summary>
        /// If the provider is connected
        /// </summary>
        Boolean Connected { get; }

        /// <summary>
        /// When the last time the provider was used or actioned upon
        /// </summary>
        DateTime LastAction { get; }

        /// <summary>
        /// Disconnect from the data source
        /// </summary>
        /// <returns>Confirmation of if it has disconnected</returns>
        Boolean Disconnect();

        /// <summary>
        /// Retrieve a set of data based on the query passed to the source
        /// </summary>
        /// <param name="command">The command to execute to retrieve the data</param>
        /// <returns>A list of data items that the reader found</returns>
        DataTable Read(String command);

        /// <summary>
        /// Write data to the provider
        /// </summary>
        /// <param name="data">The data to be written to the provider</param>
        /// <param name="command">The command to execute to manipulate the data</param>
        /// <returns>If the command was successful</returns>
        Boolean Write(DataTable data, String command);

        /// <summary>
        /// Commit the data to the destination (mainly used for file types, not connected types)
        /// </summary>
        /// <returns></returns>
        Boolean Commit();

        /// <summary>
        /// Look at the data source and try and represent the source as a dataset without a definition
        /// </summary>
        /// <returns>A representation of the data</returns>
        DataItemDefinition Analyse(AnalyseRequest<Object> request);
    }
}
