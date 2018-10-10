using System;
using Xunit;
using AutoMapper;
using TNDStudios.DataPortals.UI;
using TNDStudios.DataPortals.UI.Models;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.UI.Models.Api;
using System.Globalization;
using System.Text;

namespace TNDStudios.DataPortals.Tests.UI
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class APIProviderTestsFixture : IDisposable
    {
        public DataProviderFactory Factory; // The provider factory to test

        /// <summary>
        /// Configure the test fixture
        /// </summary>
        public APIProviderTestsFixture()
        {
            Factory = new DataProviderFactory();
        }

        /// <summary>
        /// Dispose of the factory class etc.
        /// </summary>
        public void Dispose()
        {
            Factory = null; // Kill the reference
        }
    }

    public class APIProviderTests : IClassFixture<APIProviderTestsFixture>
    {
        private APIProviderTestsFixture fixture; // Reference for the test fixture

        /// <summary>
        /// Constructor to inject the fixture
        /// </summary>
        /// <param name="data"></param>
        public APIProviderTests(APIProviderTestsFixture data)
            => fixture = data;

        [Fact]
        public void Get_FlatFile_Provider()
        {
            // Arrange

            // Assert

            // Act
        }
    }
}
