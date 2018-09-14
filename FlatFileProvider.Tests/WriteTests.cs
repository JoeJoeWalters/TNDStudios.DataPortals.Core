using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;

namespace TNDStudios.DataPortals.Tests.FlatFile
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
            DataTable dataToWrite = TestHelper.PopulateDataTable(TestHelper.TestFile_WriteTests); // Get the data

            // Act

            // Assert
            Assert.True(dataToWrite.Rows.Count != 0);
        }
    }
}
