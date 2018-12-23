using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using TNDStudios.DataPortals.Helpers;
using TNDStudios.DataPortals.PropertyBag;
using TNDStudios.DataPortals.Security;

namespace TNDStudios.DataPortals.Data
{
    public class DataProviderBase : IDataProvider
    {
        /// <summary>
        /// Can this provider read (as a data source)
        /// </summary>
        public virtual Boolean CanRead { get; set; }

        /// <summary>
        /// Can this provider write (as a data destination)
        /// </summary>
        public virtual Boolean CanWrite { get; set; }

        /// <summary>
        /// Can this provider expose analysis services (to reveal the 
        /// structure of the object being read)
        /// </summary>
        public virtual Boolean CanAnalyse { get; set; }

        /// <summary>
        /// Can this provider list objects to connect to 
        /// </summary>
        public virtual Boolean CanList { get; set; }

        /// <summary>
        /// The property bag types that can be used to define this connection
        /// </summary>
        public virtual List<PropertyBagItemType> PropertyBagTypes() => new List<PropertyBagItemType>() { };

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataProviderBase()
        {
            // Defaults
            this.CanAnalyse = true;
            this.CanList = false;
            this.CanRead = true;
            this.CanWrite = false;

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
        public DataConnection Connection { get; set; }

        /// <summary>
        /// Is the flat file provider connected to it's source?
        /// </summary>
        protected internal Boolean connected;
        public virtual Boolean Connected
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
        /// <param name="connection">The connection</param>
        /// <returns>If the system connected ok</returns>
        public virtual bool Connect(DataItemDefinition definition, DataConnection connection)
            => throw new NotImplementedException();

        /// <summary>
        /// Connect to this connection
        /// </summary>
        /// <param name="definition">The definition of the data in the connection</param>
        /// <param name="connection">The connection</param>
        /// <param name="stream">The stream to connect to</param>
        /// <returns>If the system connected ok</returns>
        public virtual bool Connect(DataItemDefinition definition, DataConnection connection, Stream stream)
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
        public virtual Boolean Test(DataConnection connection)
        {
            // Get the initial connection result
            Boolean result = this.Connect(null, connection);
            if (result)
                this.Disconnect(); // If we managed to connect, now disconnect

            return result; // Return the result
        }

        /// <summary>
        /// Get a list of the objects that this provider can read
        /// </summary>
        public virtual List<KeyValuePair<String, String>> ObjectList()
            => throw new NotImplementedException();
    }
}
