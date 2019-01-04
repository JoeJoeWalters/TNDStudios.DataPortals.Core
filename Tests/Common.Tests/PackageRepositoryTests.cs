using System;
using Xunit;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Helpers;
using System.Collections.Generic;
using TNDStudios.DataPortals.Repositories;

namespace TNDStudios.DataPortals.Tests.Common
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class PackageRepositoryTestsFixture : IDisposable
    {
        public IPackageRepository Repository; // The repostory to test

        /// <summary>
        /// Configure the test fixture
        /// </summary>
        public PackageRepositoryTestsFixture()
            => Initialise();

        public void Initialise()
            => Repository = new MemoryPackageRepository();

        /// <summary>
        /// Dispose of the repository class etc.
        /// </summary>
        public void Dispose()
        {
            Repository = null; // Kill the reference
        }
    }

    public class PackageRepositoryTests : IClassFixture<PackageRepositoryTestsFixture>
    {
        private PackageRepositoryTestsFixture fixture; // Reference for the test fixture

        /// <summary>
        /// Constructor to inject the fixture
        /// </summary>
        /// <param name="data"></param>
        public PackageRepositoryTests(PackageRepositoryTestsFixture data)
            => fixture = data;
        
        /// <summary>
        /// Test to save a package to the repository
        /// </summary>
        [Fact]
        public void PackageRepository_Save_Package()
        {
            // Arrange
            fixture.Initialise(); // Reset the fixture
            Package package = new Package() { }; // Create a new package to save

            // Act
            Package savedPackage = fixture.Repository.Save(package);

            // Assert
            Assert.NotNull(savedPackage);
            Assert.NotEqual(savedPackage.Id, Guid.Empty);
            Assert.Equal(savedPackage.Id, package.Id);
        }

        /// <summary>
        /// Test getting a single package from the repository
        /// </summary>
        [Fact]
        public void PackageRepository_Get_Package()
        {
            // Arrange
            fixture.Initialise(); // Reset the fixture
            Package package = new Package() { }; // Create a new package to save

            // Act
            Package savedPackage = fixture.Repository.Save(package);
            Package retrievedPackage = fixture.Repository.Get(savedPackage.Id);

            // Assert
            Assert.NotNull(retrievedPackage);
            Assert.NotEqual(retrievedPackage.Id, Guid.Empty);
        }

        /// <summary>
        /// Test getting a list of packages from the repository
        /// </summary>
        [Fact]
        public void PackageRepository_Get_PackageList()
        {
            // Arrange
            fixture.Initialise(); // Reset the fixture
            Int32 packageCount = 100; // Save 100 packages
           
            // Act
            for (var packageId = 0; packageId < packageCount; packageId ++)
                fixture.Repository.Save(new Package() { }); // Save another package
            List<Package> packages = fixture.Repository.Get(); // Get all packages

            // Assert
            Assert.Equal(packages.Count, packageCount);
        }        
    }
}
