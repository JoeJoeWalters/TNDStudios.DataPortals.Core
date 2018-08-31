using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Core.Data
{
    /// <summary>
    /// Interface to define how a data repository should behave
    /// </summary>
    /// <typeparam name="DI">The type of data item to handle</typeparam>
    /// <typeparam name="DP">The data provider that will facilitate the data gathering</typeparam>
    public interface 
        IDataRepository<DI, DP> 
        where DI : IDataItem
        where DP : IDataProvider
    {
        Boolean Save(IEnumerable<DI> items);
        IEnumerable<DI> Get(String query);
        Boolean Delete(IEnumerable<DI> item);
    }
}
