using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.PropertyBag;
using Xunit;

namespace TNDStudios.DataPortals.Tests.FixedWidthFile
{
    /// <summary>
    /// Testing reading date strings in various formats
    /// </summary>
    public class DateTests
    {
        /// <summary>
        /// Check that dates can be read using a specified format no matter the culture given
        /// </summary>
        [Fact]
        public void Custom_Dates_Can_Be_Read()
        {
            // Arrange
            TestHelper testHelper = new TestHelper();
            DataConnection connection = testHelper.TestConnection();
            PropertyBagHelper propertyBagHelper = new PropertyBagHelper(connection.PropertyBag);
            propertyBagHelper.Set<Int32>(PropertyBagItemTypeEnum.RowsToSkip, 1);
            propertyBagHelper.Set<Boolean>(PropertyBagItemTypeEnum.HasHeaderRecord, true);

            // Act
            DataTable data = testHelper.PopulateDataTable(TestHelper.TestFile_GenericFixedWidth, connection); // Get the data

            // Assert
            Assert.True(data.Rows.Count != 0); // It actually got some data rows
            Object row0Date = data.Rows[0]["PostDate"];
            Object row1Date = data.Rows[1]["PostDate"];
            Object row2Date = data.Rows[2]["PostDate"];
            Object row3Date = data.Rows[3]["PostDate"];

            // "10/17/2012"
            Assert.True(
                (row0Date != DBNull.Value) &&
                ((DateTime)row0Date).Day == 17 &&
                ((DateTime)row0Date).Month == 10 &&
                ((DateTime)row0Date).Year == 2012);

            // "01/14/2013"
            Assert.True(
                (row1Date != DBNull.Value) &&
                ((DateTime)row1Date).Day == 14 &&
                ((DateTime)row1Date).Month == 1 &&
                ((DateTime)row1Date).Year == 2013);

            // "XX/YY/2012"
            Assert.True((row2Date == DBNull.Value));

            // "11/21/2012"
            Assert.True(
                (row3Date != DBNull.Value) &&
                ((DateTime)row3Date).Day == 21 &&
                ((DateTime)row3Date).Month == 11 &&
                ((DateTime)row3Date).Year == 2012);
        }
    }
}
