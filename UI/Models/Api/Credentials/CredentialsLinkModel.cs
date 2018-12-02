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
        /// Filter column added to data to filter this set of credentials and the data that it can see
        /// </summary>
        public String Filter { get; set; }

        /// <summary>
        /// Can create objects
        /// </summary>
        public Boolean CanCreate { get; set; }

        /// <summary>
        /// Can delete objects
        /// </summary>
        public Boolean CanRead { get; set; }

        /// <summary>
        /// Can update objects
        /// </summary>
        public Boolean CanUpdate { get; set; }

        /// <summary>
        /// Can delete objects
        /// </summary>
        public Boolean CanDelete { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CredentialsLinkModel()
        {
            // No credentials assigned by default
            Credentials = new KeyValuePair<Guid, String>(Guid.Empty, "");

            // Default CRUD operations
            CanCreate = false;
            CanDelete = false;
            CanRead = true;
            CanUpdate = false;

            // No filter by default
            Filter = String.Empty;
        }
    }
}
