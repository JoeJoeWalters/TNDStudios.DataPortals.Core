using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// Implementation of a provider that reads / writes fixed width files
    /// </summary>
    public class FixedWidthFileProvider : DataProviderBase, IDataProvider
    {
        public FixedWidthFileProvider() : base()
        {
        }
    }
}
