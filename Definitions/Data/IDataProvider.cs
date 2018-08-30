using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Core.Data
{
    public interface IDataProvider
    {
        String ConnectionString { get; }

        Boolean Connect(String connectionString);
        Boolean Disconnect();

        IEnumerable<IDataItem> ExecuteReader(String command);
        IDataItem ExecuteScalar(String command);
        Boolean ExecuteNonQuery(String command);
    }
}
