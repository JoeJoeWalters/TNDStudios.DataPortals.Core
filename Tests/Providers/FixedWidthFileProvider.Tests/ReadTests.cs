using System;
using Xunit;
using System.Data;
using TNDStudios.DataPortals.Helpers;
using TNDStudios.DataPortals.Data;

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
            DataTable data = (new TestHelper()).PopulateDataTable(TestHelper.TestFile_DataTypes); // Get the data
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

            // Act
            DataTable data = (new TestHelper()).PopulateDataTable(TestHelper.TestFile_Headers); // Get the data
            DataRow dataRow = (data.Rows.Count >= 2) ? data.Rows[2] : null; // Get row 3 to check the data against later

            // Assert
            Assert.True(data.Rows.Count == 5); // Should be 5 rows (6 - The Header)
            Assert.True(data.Columns.Contains("Description Header")); // Should be a column that was found even though it had no quotes
            Assert.True(dataRow != null && (String)dataRow["Description Header"] == "Description 3"); // The third row should have some data for the unquoted header
        }

        /// <summary>
        /// Analyse a test string to determine the number of columns using the analysis code
        /// </summary>
        [Fact]
        public void Analyse_Column_Numbers_From_String()
        {
            // Arrange
            String testData = (new TestHelper()).GetResourceString(TestHelper.TestFile_Headers);

            // Act
            DataItemDefinition definition = FixedWidthFileHelper.AnalyseText(
                new AnalyseRequest<String>()
                {
                    Data = testData
                }
                );

            // Assert
            Assert.Equal(definition.ItemProperties.Count, (int)3);
        }

        [Fact]
        public void Analyse_Column_DataType_From_String()
        {
            // Arrange
            String testData = (new TestHelper()).GetResourceString(TestHelper.TestFile_GenericFixedWidth);

            // Act
            DataItemDefinition definition = FixedWidthFileHelper.AnalyseText(
                new AnalyseRequest<string>()
                {
                    Data = testData
                });

            // Assert
            Assert.Equal(definition.ItemProperties.Count, (int)5);
            Assert.Equal(typeof(String), definition.ItemProperties[0].DataType);
            Assert.Equal(typeof(DateTime), definition.ItemProperties[1].DataType);
            Assert.Equal(typeof(int), definition.ItemProperties[2].DataType);
            Assert.Equal(typeof(String), definition.ItemProperties[3].DataType);
            Assert.Equal(typeof(int), definition.ItemProperties[4].DataType);
        }

    }
}
