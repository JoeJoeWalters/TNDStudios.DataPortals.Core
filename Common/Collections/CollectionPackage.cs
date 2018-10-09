using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// Collection file format for saving or storing the 
    /// definitions for a "Data Portals File"
    /// </summary>
    public class CollectionPackage
    {
        /// <summary>
        /// List of definitions for this package
        /// </summary>
        public List<DataItemDefinition> Definitions { get; set; }

        /// <summary>
        /// List of connections for this package
        /// (Essentially provider to connection string)
        /// </summary>
        public List<DataConnection> Connections { get; set; }
    }
}
