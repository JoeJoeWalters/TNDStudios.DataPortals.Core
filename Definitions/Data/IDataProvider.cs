using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Core.Data
{
    /// <summary>
    /// Interface to define how a data provider should behave
    /// </summary>
    public interface IDataProvider
    {
        String ConnectionString { get; }

        Boolean Connect(String connectionString);
        Boolean Connected { get; }
        Boolean Disconnect();

        IEnumerable<IDataItem> ExecuteReader(IDataItemDefinition definition, String command);
        IDataItem ExecuteScalar(IDataItemDefinition definition, String command);
        Boolean ExecuteNonQuery(String command);
    }
}
