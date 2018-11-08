using System;
using Xunit;
using System.Reflection;
using System.IO;
using TNDStudios.DataPortals.Data;
using System.Data;
using System.Globalization;

namespace TNDStudios.DataPortals.Tests.FixedWidthFile
{
    public class KeyAndMergeTests
    {
        [Fact]
        public void Merge_Files_2_Part_Primary_Key()
        {
            // Arrange
            TestHelper testHelper = new TestHelper();

            DataItemDefinition definition = testHelper.TestDefinition(TestHelper.TestFile_GenericFixedWidth); // Get the test definition of what to merge from (but also to)

            DataTable baseData = testHelper.PopulateDataTable(TestHelper.TestFile_GenericFixedWidth); // Get the data
            DataTable mergeData = testHelper.PopulateDataTable(TestHelper.TestFile_GenericFixedWidth); // Get the data

            Stream testStream = new MemoryStream(); // A blank stream to write data to
            IDataProvider provider = new FixedWidthFileProvider(); // A flat file provider to use to write the data

            // Act
            provider.Connect(definition, testStream); // Connect to the blank stream
            provider.Write(baseData, ""); // Write the data to the empty stream
            provider.Write(mergeData, ""); // Write some more records with some updates and some adds
            DataTable mergedData = provider.Read(""); // Get the new data set back

            // Assert
            Assert.True(mergedData.Rows.Count == 6); // Expect of the total of 8 rows, 2 should merge
        }

        [Fact]
        public void Filter_Records_With_Command()
        {
            // Arrange
            TestHelper testHelper = new TestHelper();

            DataItemDefinition definition = testHelper.TestDefinition(TestHelper.TestFile_GenericFixedWidth); // Get the test definition of what to data to filter
            DataTable unfilteredData = testHelper.PopulateDataTable(TestHelper.TestFile_GenericFixedWidth); // Get the data
            
            Stream testStream = new MemoryStream(); // A blank stream to write data to
            IDataProvider provider = new FixedWidthFileProvider(); // A flat file provider to use to write the data

            String command = "[GL Account] = '3930621977'"; // The command to do the filter

            // Act
            provider.Connect(definition, testStream); // Connect to the blank stream
            provider.Write(unfilteredData, ""); // Write the unfiltered data to the empty stream
            DataTable filteredData = provider.Read(command); // Get the new data set back that has been filtered

            // Assert
            Assert.True(filteredData.Rows.Count == 1); // Expect 1 row in the filtered set
        }
    }
}
