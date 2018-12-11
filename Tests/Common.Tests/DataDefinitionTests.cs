using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TNDStudios.DataPortals.Data;
using Xunit;

namespace TNDStudios.DataPortals.Tests.Common
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class DataDefinitionTestsFixture : IDisposable
    {
        public DataItemDefinition Definition; // The definition to test
        public DataTable Data; // The data table to test

        /// <summary>
        /// Configure the test fixture
        /// </summary>
        public DataDefinitionTestsFixture()
            => Initialise();

        /// <summary>
        /// Initialise the data
        /// </summary>
        public void Initialise()
        {
            // Create a new datatable to test converting from and checking
            // as a reference once converted in to
            Data = new DataTable()
            {
            };

            // Create a new data item definition to test converting from and
            // checking as a reference once converted in to
            Definition = new DataItemDefinition()
            {
            };
        }

        /// <summary>
        /// Dispose of the repository class etc.
        /// </summary>
        public void Dispose()
        {
            // Kill the references
            Definition = null;
            Data = null;
        }
    }

    public class DataDefinitionTests : IClassFixture<DataDefinitionTestsFixture>
    {
        private DataDefinitionTestsFixture fixture; // Reference for the test fixture

        /// <summary>
        /// Constructor to inject the fixture
        /// </summary>
        /// <param name="data"></param>
        public DataDefinitionTests(DataDefinitionTestsFixture data)
            => fixture = data;

        [Fact]
        public void Convert_DataTable_To_DataDefinition()
        {
            // Arrange

            // Act

            // Assert
            Assert.True(false); // Fail always until the test is finished
        }

        [Fact]
        public void Convert_DataDefinition_ToDataTable()
        {
            // Arrange

            // Act

            // Assert
            Assert.True(false); // Fail always until the test is finished
        }
    }
}
