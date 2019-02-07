using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Request to generate a basic authentication token
    /// </summary>
    public class BasicAuthTokenRequest
    {
        /// <summary>
        /// The username needed for the basic auth token
        /// </summary>
        public String Username { get; set; } = String.Empty;

        /// <summary>
        /// The password needed for the basic auth token
        /// </summary>
        public String Password { get; set; } = String.Empty;
    }
}
