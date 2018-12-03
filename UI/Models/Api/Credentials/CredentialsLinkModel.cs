using System;
using System.Collections.Generic;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Link from an object to a set of credentials (which also holds if 
    /// the credentials are ready only, etc.
    /// </summary>
    public class CredentialsLinkModel
    {
        /// <summary>
        /// The linked credentials
        /// </summary>
        public KeyValuePair<Guid, String> Credentials { get; set; }

        /// <summary>
        /// Permissions for this set of credentials
        /// </summary>
        public PermissionsModel Permissions { get; set; }
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CredentialsLinkModel()
        {
            // No credentials assigned by default
            Credentials = new KeyValuePair<Guid, String>(Guid.Empty, "");

            // No permissions by default
            Permissions = new PermissionsModel();
        }
    }
}
