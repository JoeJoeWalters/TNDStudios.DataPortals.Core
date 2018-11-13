using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNDStudios.DataPortals.UI.Models.Api;

namespace TNDStudios.DataPortals.UI.Models.RequestResponse
{
    /// <summary>
    /// Model used for analysing and representing the data gathered
    /// data to the user / UI
    /// </summary>
    public class DataItemModel
    {
        /// <summary>
        /// Definition of the data
        /// </summary>
        public DataItemDefinitionModel Definition { get; set; }

        /// <summary>
        /// Data derived from the definition
        /// </summary>
        public DataItemValuesModel Values { get; set; }
    }
}
