using System;
using System.Collections.Generic;

namespace TNDStudios.DataPortals.Core.Data
{
    public class MSSQLProvider : IDataProvider
    {
        public string ConnectionString => throw new NotImplementedException();

        public bool Connected => throw new NotImplementedException();

        public bool Connect(string connectionString)
        {
            throw new NotImplementedException();
        }

        public bool Disconnect()
        {
            throw new NotImplementedException();
        }

        public bool ExecuteNonQuery(string command)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDataItem> ExecuteReader(string command)
        {
            throw new NotImplementedException();
        }

        public IDataItem ExecuteScalar(string command)
        {
            throw new NotImplementedException();
        }
    }
}
