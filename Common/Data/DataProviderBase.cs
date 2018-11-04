using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using TNDStudios.DataPortals.Helpers;

namespace TNDStudios.DataPortals.Data
{
    public class DataProviderBase : IDataProvider
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataProviderBase()
        {
            this.MarkLastAction(); // Tell the provider that it did something
        }

        /// <summary>
        /// Is the provider in test mode (for example, in test flat files
        /// we want to write to the internal buffer but not to disk)
        /// </summary>
        public virtual Boolean TestMode { get; set; }

        /// <summary>
        /// Provide a read-only view of the connection string
        /// </summary>
        protected internal String connectionString;
        public String ConnectionString => connectionString;

        /// <summary>
        /// Is the flat file provider connected to it's source?
        /// </summary>
        protected internal Boolean connected;
        public Boolean Connected
        {
            get { return connected; }
        }

        /// <summary>
        /// When the last time the provider was used or actioned upon
        /// </summary>
        protected internal DateTime lastAction;
        public DateTime LastAction
        {
            get { return lastAction; }
        }

        /// <summary>
        /// The default or actual object name that the provider is pointing to
        /// such as a table or spreadsheet tab
        /// </summary>
        public virtual string ObjectName { get; set; }

        /// <summary>
        /// Mark the last time something happened on a provider
        /// </summary>
        public virtual void MarkLastAction() { this.lastAction = DateTime.Now; }

        /// <summary>
        /// Connect to this connection
        /// </summary>
        /// <param name="definition">The definition of the data in the connection</param>
        /// <param name="connectionString">The connection string to connect to</param>
        /// <returns>If the system connected ok</returns>
        public virtual bool Connect(DataItemDefinition definition, String connectionString)
            => throw new NotImplementedException();

        /// <summary>
        /// Connect to this connection
        /// </summary>
        /// <param name="definition">The definition of the data in the connection</param>
        /// <param name="stream">The stream to connect to</param>
        /// <returns>If the system connected ok</returns>
        public virtual bool Connect(DataItemDefinition definition, Stream stream)
            => throw new NotImplementedException();

        /// <summary>
        /// Disconnect from the connection
        /// </summary>
        /// <returns>If the disconnect was successful</returns>
        public virtual bool Disconnect()
            => throw new NotImplementedException();

        /// <summary>
        /// Write some data to the connection
        /// </summary>
        /// <param name="data">The data to be written</param>
        /// <param name="command">The command to be performed as the write</param>
        /// <returns>If the write was successful</returns>
        public virtual bool Write(DataTable data, string command)
            => throw new NotImplementedException();

        /// <summary>
        /// Read data from the connection
        /// </summary>
        /// <param name="command">The command to run against the read</param>
        /// <returns>The data that was pulled</returns>
        public virtual DataTable Read(string command)
            => throw new NotImplementedException();

        /// <summary>
        /// Commit data to the connection
        /// </summary>
        /// <returns>If the data committed or not</returns>
        public virtual Boolean Commit()
            => throw new NotImplementedException();

        /// <summary>
        /// Analyse the connection to build a data item definition
        /// </summary>
        /// <param name="request">The parameters of the connection request</param>
        /// <returns>A data item definition based on the requested connection</returns>
        public virtual DataItemDefinition Analyse(AnalyseRequest<Object> request)
            => throw new NotImplementedException();

        /// <summary>
        /// Test a connection string for this provider type
        /// </summary>
        /// <returns>Success or failure of the connection string test</returns>
        public virtual Boolean Test(String connectionString)
        {
            // Get the initial connection result
            Boolean result = this.Connect(null, connectionString);
            if (result)
                this.Disconnect(); // If we managed to connect, now disconnect

            return result; // Return the result
        }
    }
}
