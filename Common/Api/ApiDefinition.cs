using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Api
{
    /// <summary>
    /// Define how an API Endpoint 
    /// </summary>
    public class ApiDefinition : CommonObject
    {
        /// <summary>
        /// Pointer to the definition to use
        /// </summary>
        public Guid DataDefinition { get; set; }

        /// <summary>
        /// Pointer to the data connection to use
        /// </summary>
        public Guid DataConnection { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApiDefinition() : base()
        {

        }
    }
}
