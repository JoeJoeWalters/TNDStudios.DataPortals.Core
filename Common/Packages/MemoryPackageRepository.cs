using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TNDStudios.DataPortals.Repositories
{
    public class MemoryPackageRepository : IPackageRepository
    {
        /// <summary>
        /// Internal list of packages
        /// </summary>
        private List<Package> packages;

        /// <summary>
        /// Get a list of all of the available repositories
        /// </summary>
        public List<Package> Get()
            => packages;

        /// <summary>
        /// Get a specific collection package
        /// </summary>
        /// <param name="id">The Id of the package</param>
        /// <returns>The requested package if it exists</returns>
        public Package Get(Guid id)
            => packages.Where(package => package.Id == id).FirstOrDefault();

        /// <summary>
        /// Save a collection package and return the package back once completed
        /// possibly with the new Id 
        /// </summary>
        /// <param name="package">The package to be saved</param>
        /// <returns>The package that has been saved</returns>
        public Package Save(Package package)
        {
            // Does this item already exist in this repository?
            Package existingPackage = Get(package.Id);
            if (existingPackage != null)
                existingPackage = package; // Replace with the saved package
            else
            {
                package.Id = (package.Id == Guid.Empty) ? Guid.NewGuid() : package.Id; // Give the package a new Guid as it doesn'g have one
                packages.Add(package); // Add the package
                existingPackage = package; // It now exists so assign the reference
            }

            // Return the saved (now existing) package
            return existingPackage;
        }

        /// <summary>
        /// Delete a package with a given Id
        /// </summary>
        /// <param name="id">The Id of the package to be deleted</param>
        /// <returns>A success or failure flag</returns>
        public Boolean Delete(Guid id)
        {
            // Check the package is gone
            Package package = Get(id);
            if (package != null)
            {
                // Delete the package
                packages.Remove(package);

                // Check the package is gone
                return (Get(id) == null);
            }
            else
                return true; // Already been deleted
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MemoryPackageRepository()
        {
            packages = new List<Package>(); // Create the new list by default
        }
    }
}
