using System;
using Xunit;
using System.Reflection;
using System.IO;
using TNDStudios.DataPortals.Data;
using System.Data;
using System.Globalization;

namespace TNDStudios.DataPortals.Tests.DelimitedFile
{
    public class CalculationTests
    {
        [Fact]
        public void Read_Then_Auto_Calculate()
        {
            // Arrange

            // Act
            DataTable data = (new TestHelper()).PopulateDataTable(TestHelper.TestFile_ExpressionTests); // Get the data

            // Assert
            Assert.NotEqual(0, data.Rows.Count);
            foreach (DataRow row in data.Rows)
            {
                Assert.Equal(row["Result"], (Double.Parse(row["Value"].ToString()) * (Double)row["Multiplier"]));
            }
        }
    }
}
