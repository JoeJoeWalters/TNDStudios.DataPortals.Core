using System;
using Xunit;
using System.Data;
using TNDStudios.DataPortals.Helpers;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.PropertyBag;

namespace TNDStudios.DataPortals.Tests.FixedWidthFile
{
    public class ReadTests
    {
        /// <summary>
        /// Test to make sure that each column correctly identifies a
        /// different data type when reading from a flat file 
        /// There are other tests for format specific items (such as date formats)
        /// </summary>
        [Fact]
        public void Data_Types_Can_Be_Read()
        {
            // Arrange

            // Act
            DataTable data = (new TestHelper()).PopulateDataTable(TestHelper.TestFile_GenericFixedWidth); // Get the data
            DataRow dataRow = (data.Rows.Count >= 2) ? data.Rows[2] : null; // Get row 3 to check the data against later

            // Assert
            Assert.True(data.Rows.Count != 0); // It actually got some data rows
        }

        /// <summary>
        /// Test to make sure that headers are picked up when the file is identified 
        /// as having a header and that the quoted and unquoted header definitions
        /// pick up data from the correct columns
        /// </summary>
        [Fact]
        public void Data_Read_From_Headers()
        {
            // Arrange
            TestHelper testHelper = new TestHelper();

            DataConnection connection = testHelper.TestConnection(); // Get a test connection
            PropertyBagHelper propertyBagHelper = new PropertyBagHelper(connection.PropertyBag);
            propertyBagHelper.Set<Int32>(PropertyBagItemTypeEnum.RowsToSkip, 1);
            propertyBagHelper.Set<Boolean>(PropertyBagItemTypeEnum.HasHeaderRecord, true);
            
            // Act
            DataTable data = testHelper.PopulateDataTable(TestHelper.TestFile_GenericFixedWidth, connection); // Get the data
            DataRow dataRow = (data.Rows.Count >= 1) ? data.Rows[0] : null; // Get row 3 to check the data against later

            // Assert
            Assert.True(data.Rows.Count == 530); // Should be 530 rows (532 including the header and spacer)
            Assert.True(data.Columns.Contains("Description")); // Should be a column that was found even though it had no quotes
            Assert.True(dataRow != null && ((String)dataRow["Description"]).Trim() == "TXNPUES"); // The third row should have some data for the unquoted header
            Assert.True(dataRow != null && ((String)dataRow["Ref"]).Trim() == "RHMXWPCP"); // The third row should have some data for the unquoted header
            Assert.True(dataRow != null && (Boolean)dataRow["Post"]); // The third row should have some data for the unquoted header
        }
    }
}
