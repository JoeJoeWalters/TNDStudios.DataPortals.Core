using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Model to reflect the permissions model in the domain
    /// </summary>
    public class PermissionsModel
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
        public PermissionsModel()
        {
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
