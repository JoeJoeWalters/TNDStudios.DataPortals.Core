using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;

namespace TNDStudios.DataPortals.Tests.FlatFile
{
    /// <summary>
    /// Testing reading date strings in various formats
    /// </summary>
    public class DateTests
    {
        /// <summary>
        /// Check that dates in an ISO format can be read using a specified culture
        /// </summary>
        [Fact]
        public void ISO_Dates_Can_Be_Read()
        {
            // Arrange

            // Act
            DataTable data = TestHelper.PopulateDataTable(TestHelper.TestFile_ISODates); // Get the data

            // Assert
            Assert.True(data.Rows.Count != 0); // It actually got some data rows
            Object row0Date = data.Rows[0]["Date"];
            Object row1Date = data.Rows[1]["Date"];
            Object row2Date = data.Rows[2]["Date"];
            Object row3Date = data.Rows[3]["Date"];
            Object row4Date = data.Rows[4]["Date"];

            // "2018-09-13"
            Assert.True(
                (row0Date != DBNull.Value) &&
                ((DateTime)row0Date).Day == 13 &&
                ((DateTime)row0Date).Month == 9 &&
                ((DateTime)row0Date).Year == 2018);

            // "2014-12-01"
            Assert.True(
                (row1Date != DBNull.Value) &&
                ((DateTime)row1Date).Day == 1 &&
                ((DateTime)row1Date).Month == 12 &&
                ((DateTime)row1Date).Year == 2014);

            // "2001-31-31"
            Assert.True((row2Date == DBNull.Value));

            // "0001-12-01"
            Assert.True(
                (row3Date != DBNull.Value) &&
                ((DateTime)row3Date).Day == 1 &&
                ((DateTime)row3Date).Month == 12 &&
                ((DateTime)row3Date).Year == 1);

            // "2015-01-02"
            Assert.True(
                (row4Date != DBNull.Value) &&
                ((DateTime)row4Date).Day == 2 &&
                ((DateTime)row4Date).Month == 1 &&
                ((DateTime)row4Date).Year == 2015);
        }

        /// <summary>
        /// Check that dates can be read using a specified format no matter the culture given
        /// </summary>
        [Fact]
        public void Custom_Dates_Can_Be_Read()
        {
            // Arrange

            // Act
            DataTable data = TestHelper.PopulateDataTable(TestHelper.TestFile_CustomDates); // Get the data

            // Assert
            Assert.True(data.Rows.Count != 0); // It actually got some data rows
            Object row0Date = data.Rows[0]["Date"];
            Object row1Date = data.Rows[1]["Date"];
            Object row2Date = data.Rows[2]["Date"];
            Object row3Date = data.Rows[3]["Date"];
            Object row4Date = data.Rows[4]["Date"];

            // "13 Sep 2018"
            Assert.True(
                (row0Date != DBNull.Value) &&
                ((DateTime)row0Date).Day == 13 &&
                ((DateTime)row0Date).Month == 9 &&
                ((DateTime)row0Date).Year == 2018);

            // "01 Dec 2014"
            Assert.True(
                (row1Date != DBNull.Value) &&
                ((DateTime)row1Date).Day == 1 &&
                ((DateTime)row1Date).Month == 12 &&
                ((DateTime)row1Date).Year == 2014);

            // "31 XXX 2001"
            Assert.True((row2Date == DBNull.Value));

            // "01 Dec 0001"
            Assert.True(
                (row3Date != DBNull.Value) &&
                ((DateTime)row3Date).Day == 1 &&
                ((DateTime)row3Date).Month == 12 &&
                ((DateTime)row3Date).Year == 1);

            // "02 Jan 2015"
            Assert.True(
                (row4Date != DBNull.Value) &&
                ((DateTime)row4Date).Day == 2 &&
                ((DateTime)row4Date).Month == 1 &&
                ((DateTime)row4Date).Year == 2015);
        }
    }
}
