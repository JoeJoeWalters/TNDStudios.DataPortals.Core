using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Core.Data
{
    public interface 
        IDataRepository<DI, DP> 
        where DI : IDataItem
        where DP : IDataProvider
    {
        Boolean Connect(DP dataProvider, String connectionString);
        Boolean Disconnect();

        Boolean Save(IEnumerable<DI> items);
        IEnumerable<DI> Get(String query);
        Boolean Delete(IEnumerable<DI> item);
    }
}
