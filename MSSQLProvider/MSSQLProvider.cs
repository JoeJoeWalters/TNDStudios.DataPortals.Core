using System;
using System.Collections.Generic;

namespace TNDStudios.DataPortals.Core.Data
{
    public class MSSQLProvider : IDataProvider
    {
        public MSSQLProvider()
        {
        }

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

        public bool ExecuteNonQuery(DataItemDefinition definition, string command)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataItem> ExecuteReader(DataItemDefinition definition, String command)
        {
            throw new NotImplementedException();
        }

        public DataItem ExecuteScalar(DataItemDefinition definition, string command) 
        {
            throw new NotImplementedException();
        }
    }
}
