using System;
using Xunit;
using System.Reflection;
using System.IO;
using TNDStudios.DataPortals.Data;
using System.Data;

namespace TNDStudios.DataPortals.Tests.Providers
{
    public class FlatFileProviderTests
    {
        // Constants for the test files (used so we can get the resouce stream
        // but also so we can abstract the creation of the data definition)
        private const String testFile_DataTypes = "TestFiles.DataTypesTest.txt";
        private const String testFile_Headers = "TestFiles.HeadersTest.txt";

        /// <summary>
        /// Test to make sure that each column correctly identifies a
        /// different data type when reading from a flat file 
        /// There are other tests for format specific items (such as date formats)
        /// </summary>
        [Fact]
        public void DataTypes_From_FileStream()
        {
            // Arrange: Get the test data from the resource in the manifest
            Stream resourceStream = GetResourceStream(testFile_DataTypes);

            // Arrage: Generate a new flat file provider and then connect it to the stream of the CSV file
            IDataProvider provider = new FlatFileProvider();
            provider.Connect(resourceStream);

            // Arrage: Provide a definition of what wants to be retrieved from the flat file
            DataItemDefinition definition = TestDefinition(testFile_DataTypes);

            // Act
            DataTable data = provider.ExecuteReader(definition, ""); // Get the data

            // Assert
            Assert.True(data.Rows.Count != 0); // It actually got some data rows
        }

        /// <summary>
        /// Test to make sure that headers are picked up when the file is identified 
        /// as having a header and that the quoted and unquoted header definitions
        /// pick up data from the correct columns
        /// </summary>
        [Fact]
        public void DataFromHeaders_From_FileStream()
        {
            // Arrange: Get the test data from the resource in the manifest
            Stream resourceStream = GetResourceStream(testFile_Headers);

            // Arrage: Generate a new flat file provider and then connect it to the stream of the CSV file
            IDataProvider provider = new FlatFileProvider();
            provider.Connect(resourceStream);

            // Arrage: Provide a definition of what wants to be retrieved from the flat file
            DataItemDefinition definition = TestDefinition(testFile_Headers);

            // Act
            DataTable data = provider.ExecuteReader(definition, ""); // Get the data
            DataRow dataRow = (data.Rows.Count >= 2) ? data.Rows[2] : null; // Get row 3 to check the data against later

            // Assert
            Assert.True(data.Rows.Count == 5); // Should be 5 rows (6 - The Header)
            Assert.True(data.Columns.Contains("Description Header")); // Should be a column that was found even though it had no quotes
            Assert.True(dataRow != null && (String)dataRow["Description Header"] == "Description 3"); // The third row should have some data for the unquoted header
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
