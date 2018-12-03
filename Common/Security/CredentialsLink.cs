using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Security
{
    /// <summary>
    /// Link from an object to a set of credentials (which also holds if 
    /// the credentials are ready only, etc.
    /// </summary>
    public class CredentialsLink
    {
        /// <summary>
        /// The linked credentials
        /// </summary>
        public Guid Credentials { get; set; }

        /// <summary>
        /// The permissions linked to this set of credentials
        /// </summary>
        public Permissions Permissions { get; set; }
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CredentialsLink()
        {
            // No credentials assigned by default
            Credentials = Guid.Empty;

            // Default Permissions
            Permissions = new Permissions();
        }
    }
}
