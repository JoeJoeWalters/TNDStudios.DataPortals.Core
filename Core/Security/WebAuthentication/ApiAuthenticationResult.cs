using System;
using System.Net;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Repositories;

namespace TNDStudios.DataPortals.Security
{
    /// <summary>
    /// Object used to pass a result for an API request back to the 
    /// calling method with a set of data the caller will need to 
    /// process the result
    /// </summary>
    public class ApiAuthenticationResult
    {
        /// <summary>
        /// The status code of the authentication request
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Description of the status (what happened to cause it)
        /// </summary>
        public String StatusDescription { get; set; }

        /// <summary>
        /// The package that was found as part of the managed Api Request
        /// </summary>
        public Package Package { get; set; }

        /// <summary>
        /// The Permissions that were found as part of the request
        /// </summary>
        public Permissions Permissions { get; set; }

        /// <summary>
        /// The Api Definition that was found as part of the request
        /// </summary>
        public ApiDefinition ApiDefinition { get; set; } 

        /// <summary>
        /// The Data Definition that is part of the definition
        /// </summary>
        public DataItemDefinition DataDefinition { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApiAuthenticationResult()
        {
            StatusCode = HttpStatusCode.OK; // Everything is OK by default
            StatusDescription = String.Empty; // No status description by default
            Package = null; // No package by default
            Permissions = null; // No permissions by default
            ApiDefinition = null; // No Api definition by default
        }
    }
}
