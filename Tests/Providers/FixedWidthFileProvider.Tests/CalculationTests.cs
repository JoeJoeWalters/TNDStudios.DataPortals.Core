using System;
using Xunit;
using System.Reflection;
using System.IO;
using TNDStudios.DataPortals.Data;
using System.Data;
using System.Globalization;

namespace TNDStudios.DataPortals.Tests.FixedWidthFile
{
    public class CalculationTests
    {
        [Fact]
        public void Read_Then_Auto_Calculate()
        {
            // Arrange

            // Act
            DataTable data = (new TestHelper()).PopulateDataTable(TestHelper.TestFile_GenericFixedWidth); // Get the data

            // Assert
            Assert.NotEqual(0, data.Rows.Count);
            foreach (DataRow row in data.Rows)
            {
                //"IIF(Post, Debit, 0)"
                Double.TryParse(row["Debit"].ToString(), out Double debit);
                Double.TryParse(row["Result"].ToString(), out Double result);
                Assert.Equal(result, ((Boolean)row["Post"]) ? debit : (Double)0.0);
            }
        }
    }
}
