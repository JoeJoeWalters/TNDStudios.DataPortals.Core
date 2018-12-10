using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TNDStudios.DataPortals.Helpers;
using TNDStudios.DataPortals.PropertyBag;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// Implementation of a provider that connects to a SQL Source
    /// </summary>
    public class SQLProvider : DataProviderBase, IDataProvider
    {
        /// <summary>
        /// The data definition used for this provider
        /// </summary>
        private DataItemDefinition definition;

        /// <summary>
        /// Items used to connect to the Sql Server / Sql Based Server
        /// </summary>
        private SqlConnection sqlConnection;

        /// <summary>
        /// Analyse the connection to get the definition back
        /// </summary>
        /// <param name="request">The analysis request paramaters</param>
        /// <returns>The data item definition derived from the connection and object</returns>
        public override DataItemDefinition Analyse(AnalyseRequest<object> request)
        {
            DataItemDefinition result = new DataItemDefinition(); // Empty Response By Default

            // Are we connected and have an object name?
            if ((ObjectName ?? String.Empty) != String.Empty &&
                this.Connected)
            {
                // Set up the command to run and select all columns from the object
                using (SqlCommand command = 
                    new SqlCommand($"select top {request.SampleSize.ToString()} * from {ObjectName}"))
                {
                    // Run the command
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        DataTable tempTable = new DataTable(); // Create a temporary table
                        tempTable.Load(dataReader); // Run the data reader to load the results
                        result.FromDataTable(tempTable); // Tell the definition to load itself from the data table provided
                        
                        // Clean up
                        tempTable = null;
                    }
                }
            }

            // Send the result back
            return result;
        }

        /// <summary>
        /// Can the SQL Provider analyse the connection to give information back
        /// </summary>
        public override Boolean CanAnalyse { get => true; }

        /// <summary>
        /// Can the SQL Provider read data?
        /// </summary>
        public override Boolean CanRead { get => true; }

        /// <summary>
        /// Override to the base behaviour and allow data writing
        /// </summary>
        public override Boolean CanWrite { get => true; }

        /// <summary>
        /// This provider can list objects that can be connected to
        /// </summary>
        public override Boolean CanList { get => true; }

        /// <summary>
        /// The property bag types that can be used to define this connection
        /// </summary>
        public override List<PropertyBagItemType> PropertyBagTypes() =>
            new List<PropertyBagItemType>()
            {
                new PropertyBagItemType(){ DataType = typeof(Int32), DefaultValue = 0, PropertyType = PropertyBagItemTypeEnum.RowsToSkip }
            };

        /// <summary>
        /// Is the provider connected?
        /// </summary>
        public override bool Connected
        {
            get
            {
                try
                {
                    return (sqlConnection != null && sqlConnection.State != ConnectionState.Closed);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// Override the connection object to connect to the Sql Server
        /// </summary>
        /// <param name="definition">The definition of the data to connect to</param>
        /// <param name="connection">The connect properties to connect to the server</param>
        /// <returns>If the connection worked</returns>
        public override Boolean Connect(DataItemDefinition definition, DataConnection connection)
        {
            if (!this.Connected)
            {
                // Connect to the sql server
                this.sqlConnection = new SqlConnection(connection.ConnectionString);
                try
                {
                    this.sqlConnection.Open(); // Start the connection
                    this.definition = definition; // Assign the definition
                    return true; // Success!
                }
                catch { return false; }
            }
            else
                return false;
        }

        /// <summary>
        /// Read data from the Connection
        /// </summary>
        /// <param name="command">The Sql Where Clause to filter the data</param>
        /// <returns>The data pulled from the object in Sql</returns>
        public override DataTable Read(string command)
        {
            // Create the default view of the results to return
            DataTable result = definition.ToDataTable();

            base.MarkLastAction(); // Mark the last time the command ran

            // Return the results
            return result;
        }

        /// <summary>
        /// Commit is already part of the read and write process so always return true
        /// </summary>
        /// <returns>Always true</returns>
        public override Boolean Commit() => true;

        /// <summary>
        /// Disconnect from the Sql Source
        /// </summary>
        /// <returns>If it disconnected or not</returns>
        public override Boolean Disconnect()
        {
            // Connected already?
            if (sqlConnection != null)
            {
                try
                {
                    sqlConnection.Close(); // Close the connection
                    return true; // Didn't fail so is closed
                }
                catch { return false; } // Failed to close 
            }
            else
                return true; // Not set up yet so is disconnected
        }

        /// <summary>
        /// Test the connection to the Sql Server
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public override Boolean Test(DataConnection connection)
        {
            return base.Test(connection);
        }

        /// <summary>
        /// Write data to the Sql Server / Sql Source
        /// </summary>
        /// <param name="data">The data to be written</param>
        /// <param name="command">The filter / command to write the data</param>
        /// <returns>If the write was successful</returns>
        public override Boolean Write(DataTable data, string command)
        {
            return base.Write(data, command);
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SQLProvider() : base()
        {
        }
    }
}
