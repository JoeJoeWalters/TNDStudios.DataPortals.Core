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
        where DI : DataItem
        where DP : IDataProvider
    {
        /// <summary>
        /// Save some items to the repository & data source
        /// </summary>
        /// <param name="items">The list of items to be saved</param>
        /// <returns>If the save was successful</returns>
        Boolean Save(IEnumerable<DI> items);

        /// <summary>
        /// Execute a query to get some data
        /// </summary>
        /// <param name="query">The query that will retrieve the data</param>
        /// <returns>A list of data</returns>
        IEnumerable<DI> Get(String query);

        /// <summary>
        /// Delete an item from the repository
        /// </summary>
        /// <param name="item">The item to be deleted</param>
        /// <returns>If the deletion was successful</returns>
        Boolean Delete(IEnumerable<DI> item);
    }
}
