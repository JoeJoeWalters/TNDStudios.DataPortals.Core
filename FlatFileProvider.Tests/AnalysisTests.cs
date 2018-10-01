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
        public void Analyse_Speed_Test()
        {
            // Arrange
            String file = TestHelper.GetResourceString(
                TestHelper.TestFile_BigFileSalesRecords);
            FlatFileProvider provider = new FlatFileProvider();

            // Act
            DataItemDefinition definition = provider.Analyse(file);

            // Assert
            Assert.Equal(14, definition.ItemProperties.Count);
        }

        [Fact]
        public void Read_Speed_Test()
        {
            // Arrange
            Stream file = TestHelper.GetResourceStream(
                TestHelper.TestFile_BigFileSalesRecords);
            FlatFileProvider provider = new FlatFileProvider();

            // Act
            DataItemDefinition definition = provider.Analyse(file);
            provider.Connect(definition, file);
            DataTable data = provider.Read("");

            // Assert
            Assert.Equal(5000, data.Rows.Count);
        }
    }
}
