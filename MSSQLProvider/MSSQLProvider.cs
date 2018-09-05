using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace TNDStudios.DataPortals.Data
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

        public bool Connect(Stream stream)
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

        public DataTable ExecuteReader(DataItemDefinition definition, String command)
        {
            throw new NotImplementedException();
        }

        public DataTable ExecuteScalar(DataItemDefinition definition, string command) 
        {
            throw new NotImplementedException();
        }
    }
}
