using System;
using Xunit;
using System.Data;
using TNDStudios.DataPortals.Helpers;
using TNDStudios.DataPortals.Data;
using System.IO;

namespace TNDStudios.DataPortals.Tests.FlatFile
{
    public class AnalysisTests
    {
        /// <summary>
        /// Test of 5000 sales records to make sure the analysis 
        /// is done in a timely manner
        /// </summary>
        [Fact]
        public void Analyse_BigData_ColumnCount()
        {
            // Arrange
            String file = TestHelper.GetResourceString(
                TestHelper.TestFile_BigFileSalesRecords);
            DelimitedFileProvider provider = new DelimitedFileProvider();

            // Act
            DataItemDefinition definition = provider.Analyse(
                new AnalyseRequest<object>
                {
                    Data = file
                }
                );

            // Assert
            Assert.Equal(14, definition.ItemProperties.Count);
        }

        [Fact]
        public void Analyse_BigData_RowCount()
        {
            // Arrange
            Stream file = TestHelper.GetResourceStream(
                TestHelper.TestFile_BigFileSalesRecords);
            DelimitedFileProvider provider = new DelimitedFileProvider();

            // Act
            DataItemDefinition definition = provider.Analyse(
                new AnalyseRequest<Object>()
                {
                    Data = file
                });
            provider.Connect(definition, file);
            DataTable data = provider.Read("");

            // Assert
            Assert.Equal(5000, data.Rows.Count);
        }
    }
}
