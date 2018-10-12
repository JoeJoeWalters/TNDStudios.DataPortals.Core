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
        /// Mark the last time something happened on a provider
        /// </summary>
        public virtual void MarkLastAction() { this.lastAction = DateTime.Now; }

        public virtual bool Connect(DataItemDefinition definition, String connectionString)
            => throw new NotImplementedException();

        public virtual bool Connect(DataItemDefinition definition, Stream stream)
            => throw new NotImplementedException();

        public virtual bool Disconnect()
            => throw new NotImplementedException();

        public virtual bool Write(DataTable data, string command)
            => throw new NotImplementedException();

        public virtual DataTable Read(string command)
            => throw new NotImplementedException();

        public virtual Boolean Commit()
            => throw new NotImplementedException();

        public virtual DataItemDefinition Analyse(AnalyseRequest<Object> request)
            => throw new NotImplementedException();
    }
}
