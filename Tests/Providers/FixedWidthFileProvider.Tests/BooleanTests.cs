using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;

namespace TNDStudios.DataPortals.Tests.FixedWidthFile
{
    /// <summary>
    /// Testing reading date strings in various formats
    /// </summary>
    public class BooleanTests
    {

        /// <summary>
        /// Check that the conversion of various type of boolean actually
        /// get converted to the expected result. Such as "yes", "no", "true", 1, 0 etc.
        /// </summary>
        [Fact]
        public void Boolean_Values_Of_Different_Types()
        {
            // Arrange

            // Act
            DataTable data = (new TestHelper()).PopulateDataTable(TestHelper.TestFile_GenericFixedWidth); // Get the data

            // Assert
            Assert.True(data.Rows.Count != 0); // It actually got some data rows
            Assert.True((data.Rows.Count == 5) && (Boolean)data.Rows[0]["Active"] == true); // Check row 0's expected result
            Assert.True((data.Rows.Count == 5) && (Boolean)data.Rows[1]["Active"] == true); // Check row 1's expected result
            Assert.True((data.Rows.Count == 5) && (Boolean)data.Rows[2]["Active"] == true); // Check row 2's expected result
            Assert.True((data.Rows.Count == 5) && (Boolean)data.Rows[3]["Active"] == false); // Check row 3's expected result
            Assert.True((data.Rows.Count == 5) && (Boolean)data.Rows[4]["Active"] == false); // Check row 4's expected result
        }
    }
}
