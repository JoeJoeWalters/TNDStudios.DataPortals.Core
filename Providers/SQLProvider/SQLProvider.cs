using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// Implementation of a provider that connects to a SQL Source
    /// </summary>
    public class SQLProvider : DataProviderBase, IDataProvider
    {
        public SQLProvider() : base()
        {
        }
    }
}
