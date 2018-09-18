using System;
using Xunit;
using System.Reflection;
using System.IO;
using TNDStudios.DataPortals.Data;
using System.Data;
using System.Globalization;

namespace TNDStudios.DataPortals.Tests.FlatFile
{
    public class KeyAndMergeTests
    {
        [Fact]
        public void Merge_Files_2_Part_Primary_Key()
        {
            /*
            // Arrange
            DataItemDefinition definition = TestHelper.TestDefinition(TestHelper.TestFile_PKMergeFrom); // Get the test definition of what to merge from (but also to)
            DataTable dataToWrite = TestHelper.PopulateDataTable(TestHelper.TestFile_PKMergeFrom); // Get the data
            DataTable dataToRead = null; // Table to read the data back in to (to verify it was created)
            Stream testStream = new MemoryStream(); // A blank stream to write data to
            IDataProvider provider = new FlatFileProvider(); // A flat file provider to use to write the data

            // Act
            provider.Connect(testStream); // Connect to the blank stream
            provider.Write(definition, dataToWrite, ""); // Write the data to the empty stream
            dataToRead = provider.Read(definition, ""); // Get the data back

            // Assert
            Assert.True(dataToRead.Rows.Count != 0);
            */
        }
    }
}
