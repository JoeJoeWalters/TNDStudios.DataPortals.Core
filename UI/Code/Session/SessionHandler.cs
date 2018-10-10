using System;
using System.Collections.Generic;
using System.Linq;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.UI
{
    /// <summary>
    /// Static class to handle the session items
    /// </summary>
    public static class SessionHandler
    {
        /// <summary>
        /// If the session handler has been initialised
        /// </summary>
        public static Boolean Initialised { get; set; }

        /// <summary>
        /// The Id of the current collection package
        /// </summary>
        public static Guid CurrentPackageId { get; set; }

        /// <summary>
        /// Static package to hold the activity for the website
        /// </summary>
        public static List<CollectionPackage> Packages { get; set; }

        /// <summary>
        /// Get the current package object
        /// </summary>
        public static CollectionPackage CurrentPackage =>
            Packages.Where(Package => Package.Id == CurrentPackageId)
                .FirstOrDefault();

        /// <summary>
        /// Initialise the session handler
        /// </summary>
        public static void Initialise()
        {
            // Initialised?
            if (!Initialised)
            {
#warning [By default for now just create one collection package until we think about multiple orgs / environments etc.]
                CurrentPackageId = Guid.NewGuid(); // Create a default Id

                // Set up a blank list of packages
                Packages = new List<CollectionPackage>()
                {
                    new CollectionPackage()
                    {
                        Id = CurrentPackageId,
                        Name = "Test Package",
                        Description = "Test Package Description",
                        ApiDefinitions = new List<ApiDefinition>() { },
                        DataConnections = new List<DataConnection>() { },
                        DataDefinitions = new List<DataItemDefinition>() { }
                    }
                };
            }
        }
    }
}
