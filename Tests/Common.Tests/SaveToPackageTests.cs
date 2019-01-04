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
    public class SaveToPackageTestsFixture : IDisposable
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

    public class SaveToPackageTests : IClassFixture<SaveToPackageTestsFixture>
    {
        private SaveToPackageTestsFixture fixture; // Reference for the test fixture

        /// <summary>
        /// Constructor to inject the fixture
        /// </summary>
        /// <param name="data"></param>
        public SaveToPackageTests(SaveToPackageTestsFixture data)
            => fixture = data;

        /// <summary>
        /// Test saving a connection in to a package
        /// </summary>
        [Fact]
        public void Save_Connection_To_Package()
        {

        }

        /// <summary>
        /// Test saving a data definition to a package
        /// </summary>
        [Fact]
        public void Save_DataDefinition_To_Package()
        {

        }

        /// <summary>
        /// Test saving a set of credentails to a package
        /// </summary>
        [Fact]
        public void Save_Credentials_To_Package()
        {

        }

        /// <summary>
        /// Test saving a transformation to a package
        /// </summary>
        [Fact]
        public void Save_Transformation_To_Package()
        {

        }

        /// <summary>
        /// Test saving a managed api to a package
        /// </summary>
        [Fact]
        public void Save_Api_To_Package()
        {

        }
    }
}
