using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.DataPortals.UI.Models
{
    /// <summary>
    /// Class used to defined what is needed for a view that relies on a package
    /// </summary>
    public class PackagePageVM
    {
        /// <summary>
        /// The Id for the current package
        /// </summary>
        public Guid PackageId { get; set; }
    }
}
