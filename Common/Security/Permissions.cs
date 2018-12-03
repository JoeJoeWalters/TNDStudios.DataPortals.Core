using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Security
{
    /// <summary>
    /// Basic permissions and filtering class to indicate what data / other the credentials 
    /// are allowed to use
    /// </summary>
    public class Permissions
    {
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
        public Permissions()
        {
            // Default CRUD operations
            CanCreate = false;
            CanDelete = false;
            CanRead = false;
            CanUpdate = false;

            // No filter by default
            Filter = String.Empty;
        }
    }
}
