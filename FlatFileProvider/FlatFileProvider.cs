using System;
using System.Collections.Generic;

namespace TNDStudios.DataPortals.Core.Data
{
    public class FlatFileProvider : IDataProvider
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

        public bool ExecuteNonQuery(IDataItemDefinition definition, string command)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDataItem> ExecuteReader(IDataItemDefinition definition, String command)
        {
            throw new NotImplementedException();
        }

        public IDataItem ExecuteScalar(IDataItemDefinition definition, string command)
        {
            throw new NotImplementedException();
        }
    }
}
