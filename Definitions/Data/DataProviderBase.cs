using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace TNDStudios.DataPortals.Data
{
    public class DataProviderBase : IDataProvider
    {
        /// <summary>
        /// Provide a read-only view of the connection string
        /// </summary>
        protected internal String connectionString;
        public String ConnectionString => connectionString;

        /// <summary>
        /// Is the flat file provider connected to it's source?
        /// </summary>
        protected internal Boolean connected;
        public Boolean Connected => connected;

        public virtual bool Connect(string connectionString)
        {
            throw new NotImplementedException();
        }

        public virtual bool Connect(Stream stream)
        {
            throw new NotImplementedException();
        }

        public virtual bool Disconnect()
        {
            throw new NotImplementedException();
        }

        public virtual bool ExecuteNonQuery(DataItemDefinition definition, string command)
        {
            throw new NotImplementedException();
        }

        public virtual DataTable ExecuteReader(DataItemDefinition definition, string command)
        {
            throw new NotImplementedException();
        }

        public virtual DataTable ExecuteScalar(DataItemDefinition definition, string command)
        {
            throw new NotImplementedException();
        }
    }
}
