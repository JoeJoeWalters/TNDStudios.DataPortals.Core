using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using TNDStudios.DataPortals.Helpers;
using TNDStudios.DataPortals.PropertyBag;

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

        [Description("Delimited File Provider")]
        DelimitedFileProvider = 1,

        [Description("Fixed Width File Provider")]
        FixedWidthFileProvider = 2,

        [Description("SQL Data Source")]
        SQLProvider = 3
    }

    /// <summary>
    /// Interface to define how a data provider should behave
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Can this provider read (as a data source)
        /// </summary>
        Boolean CanRead { get; }

        /// <summary>
        /// Can this provider write (as a data destination)
        /// </summary>
        Boolean CanWrite { get; }

        /// <summary>
        /// Can this provider expose analysis services (to reveal the 
        /// structure of the object being read)
        /// </summary>
        Boolean CanAnalyse { get; }

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
        /// The property bag types that can be used to define this connection
        /// </summary>
        List<PropertyBagItemType> PropertyBagTypes();

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
        /// If the provider is pointing to a specific object within the 
        /// connection (e.g. a table name or a spreadsheet tab etc.)
        /// </summary>
        String ObjectName { get; set; }

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

        /// <summary>
        /// Test a connection string for this provider type
        /// </summary>
        /// <returns>Success or failure of the connection string test</returns>
        Boolean Test(String connectionString);
    }
}
