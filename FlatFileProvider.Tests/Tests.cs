using System;
using Xunit;
using System.Reflection;
using System.IO;
using TNDStudios.DataPortals.Data;
using System.Data;
using System.Globalization;

namespace TNDStudios.DataPortals.Tests.Providers
{
    public class FlatFileProviderTests
    {
        // Constants for the test files (used so we can get the resouce stream
        // but also so we can abstract the creation of the data definition)
        private const String testFile_DataTypes = "TestFiles.DataTypesTest.txt";
        private const String testFile_Headers = "TestFiles.HeadersTest.txt";
        private const String testFile_ISODates = "TestFiles.Dates.ISODates.txt";

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
            DataTable data = PopulateDataTable(testFile_DataTypes); // Get the data
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
            DataTable data = PopulateDataTable(testFile_Headers); // Get the data
            DataRow dataRow = (data.Rows.Count >= 2) ? data.Rows[2] : null; // Get row 3 to check the data against later

            // Assert
            Assert.True(data.Rows.Count == 5); // Should be 5 rows (6 - The Header)
            Assert.True(data.Columns.Contains("Description Header")); // Should be a column that was found even though it had no quotes
            Assert.True(dataRow != null && (String)dataRow["Description Header"] == "Description 3"); // The third row should have some data for the unquoted header
        }

        /// <summary>
        /// Check that the conversion of various type of boolean actually
        /// get converted to the expected result. Such as "yes", "no", "true", 1, 0 etc.
        /// </summary>
        [Fact]
        public void Boolean_Values_Of_Different_Types()
        {
            // Arrange

            // Act
            DataTable data = PopulateDataTable(testFile_DataTypes); // Get the data

            // Assert
            Assert.True(data.Rows.Count != 0); // It actually got some data rows
            Assert.True((data.Rows.Count == 5) && (Boolean)data.Rows[0]["Active"] == true); // Check row 0's expected result
            Assert.True((data.Rows.Count == 5) && (Boolean)data.Rows[1]["Active"] == true); // Check row 1's expected result
            Assert.True((data.Rows.Count == 5) && (Boolean)data.Rows[2]["Active"] == true); // Check row 2's expected result
            Assert.True((data.Rows.Count == 5) && (Boolean)data.Rows[3]["Active"] == false); // Check row 3's expected result
            Assert.True((data.Rows.Count == 5) && (Boolean)data.Rows[4]["Active"] == false); // Check row 4's expected result
        }

        [Fact]
        public void ISO_Dates_Can_Be_Read()
        {
            // Arrange

            // Act
            DataTable data = PopulateDataTable(testFile_ISODates); // Get the data

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
        /// Generate the data set for the testing of different different types
        /// </summary>
        /// <param name="testDefinition">Which test file to load</param>
        /// <returns>The prepared data table</returns>
        private DataTable PopulateDataTable(String testDefinition)
        {
            // Get the test data from the resource in the manifest
            Stream resourceStream = GetResourceStream(testDefinition);

            // Create a new flat file provider
            IDataProvider provider = new FlatFileProvider();
            provider.Connect(resourceStream);

            // Get the test definition (The columns, data types etc. for this file)
            DataItemDefinition definition = TestDefinition(testDefinition);

            // Read the data from the provider
            DataTable data = provider.ExecuteReader(definition, ""); // Get the data

            // Return the data table
            return data;
        }

        /// <summary>
        /// Generate a test definition that is common between the different test files
        /// </summary>
        /// <returns>The common data definition</returns>
        private DataItemDefinition TestDefinition(String testDefinition)
        {
            // Arrage: Provide a definition of what wants to be retrieved from the flat file
            DataItemDefinition definition = new DataItemDefinition();

            switch (testDefinition)
            {
                case testFile_DataTypes:

                    // Definition for different data types and the data defined by ordinal position
                    definition.Properties.Add(new DataItemProperty() { Name = "Title", DataType = typeof(String), OridinalPosition = 0 });
                    definition.Properties.Add(new DataItemProperty() { Name = "CreatedDate", DataType = typeof(DateTime), OridinalPosition = 1 });
                    definition.Properties.Add(new DataItemProperty() { Name = "Size", DataType = typeof(Double), OridinalPosition = 2 });
                    definition.Properties.Add(new DataItemProperty() { Name = "Description", DataType = typeof(String), OridinalPosition = 3 });
                    definition.Properties.Add(new DataItemProperty() { Name = "Active", DataType = typeof(Boolean), OridinalPosition = 4 });
                    definition.HasHeaderRecord = false;

                    break;

                case testFile_Headers:

                    // Definition for getting the data by the name of the header
                    definition.Properties.Add(new DataItemProperty() { Name = "Title", DataType = typeof(String), OridinalPosition = -1 });
                    definition.Properties.Add(new DataItemProperty() { Name = "Description Header", DataType = typeof(String), OridinalPosition = -1 });
                    definition.Properties.Add(new DataItemProperty() { Name = "Value", DataType = typeof(String), OridinalPosition = -1 });
                    definition.HasHeaderRecord = true;

                    break;

                case testFile_ISODates:

                    // Definition for supplying a list of ISO (and bad) dates to test
                    definition.Properties.Add(new DataItemProperty() { Name = "Date", DataType = typeof(DateTime), OridinalPosition = 0 });
                    definition.HasHeaderRecord = true;
                    definition.Culture = CultureInfo.InvariantCulture;

                    break;
            }

            // Return the definition
            return definition;
        }

        /// <summary>
        /// Build a memory stream from the embedded resource to feed to the test scenarios
        /// </summary>
        /// <param name="embeddedResourceName">The name of the resource to read</param>
        /// <returns>A memory stream with the data contained within</returns>
        private Stream GetResourceStream(String embeddedResourceName)
        {
            String name = FormatResourceName(Assembly.GetExecutingAssembly(), embeddedResourceName);
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(
                    name
                    );
        }

        /// <summary>
        /// Get the resource name by deriving it from the assembly
        /// </summary>
        /// <param name="assembly">The assembly to check</param>
        /// <param name="resourceName">The name of the resource</param>
        /// <returns></returns>
        private String FormatResourceName(Assembly assembly, string resourceName)
            => assembly.GetName().Name + "." + resourceName.Replace(" ", "_")
                                                            .Replace("\\", ".")
                                                            .Replace("/", ".");
    }
}
