using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Repositories
{
    /// <summary>
    /// Interface to define how packages are saved to their relevant locations
    /// (Can be used for testing by having a memory only repository instead
    /// of physically saving the repository data to disk etc.)
    /// </summary>
    public interface IPackageRepository
    {
        /// <summary>
        /// Get a list of all of the available repositories
        /// </summary>
        List<Package> Get();

        /// <summary>
        /// Get a specific collection package
        /// </summary>
        /// <param name="id">The Id of the package</param>
        /// <returns>The requested package if it exists</returns>
        Package Get(Guid id);

        /// <summary>
        /// Save a collection package and return the package back once completed
        /// possibly with the new Id 
        /// </summary>
        /// <param name="package">The package to be saved</param>
        /// <returns>The package that has been saved</returns>
        Package Save(Package package);

        /// <summary>
        /// Delete a package with a given Id
        /// </summary>
        /// <param name="id">The Id of the package to be deleted</param>
        /// <returns>A success or failure flag</returns>
        Boolean Delete(Guid id);
    }
}
