using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using TNDStudios.DataPortals.Data;
using Xunit;

namespace TNDStudios.DataPortals.Tests.DelimitedFile
{
    /// <summary>
    /// Testing writing data to the data provider
    /// </summary>
    public class WriteTests
    {
        /// <summary>
        /// Can we write data of a given type to a stream
        /// </summary>
        [Fact]
        public void Write_DataTypes()
        {
            // Arrange
            TestHelper testHelper = new TestHelper();

            DataConnection connection = testHelper.TestConnection(); // Get a test connection
            DataItemDefinition definition = testHelper.TestDefinition(TestHelper.TestFile_WriteTests); // Get the test definition of what to write
            DataTable dataToWrite = testHelper.PopulateDataTable(TestHelper.TestFile_WriteTests); // Get the data
            DataTable dataToRead = null; // Table to read the data back in to (to verify it was created)
            Stream testStream = new MemoryStream(); // A blank stream to write data to
            IDataProvider provider = new DelimitedFileProvider(); // A flat file provider to use to write the data

            // Act
            provider.Connect(definition, connection, testStream); // Connect to the blank stream
            provider.Write(dataToWrite, ""); // Write the data to the empty stream
            dataToRead = provider.Read(""); // Get the data back

            // Assert
            Assert.True(dataToRead.Rows.Count != 0);
        }
    }
}
