using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// Transformation from one data definition to another
    /// </summary>
    public class Transformation : CommonObject
    {
        /// <summary>
        /// The source data definition reference
        /// </summary>
        public Guid Source { get; set; }

        /// <summary>
        /// The destination data definition reference
        /// </summary>
        public Guid Destination { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Transformation()
        {
        }
    }
}
