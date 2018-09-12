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
        [Fact]
        public void Read_FlatFile_From_Stream()
        {
            // Arrange: Get the test data from the resource in the manifest
            Stream resourceStream = GetResourceStream("TestFiles.CSVTest.txt");

            // Arrage: Generate a new flat file provider and then connect it to the stream of the CSV file
            IDataProvider provider = new FlatFileProvider();
            provider.Connect(resourceStream);

            // Arrage: Provide a definition of what wants to be retrieved from the flat file
            DataItemDefinition definition = TestDefinition(false);

            // Act
            DataTable data = provider.ExecuteReader(definition, "");

            // Assert
            Assert.True(data.Rows.Count != 0);
        }

        /// <summary>
        /// Generate a test definition that is common between the different test files
        /// </summary>
        /// <returns>The common data definition</returns>
        private DataItemDefinition TestDefinition(Boolean hasHeaderRecord)
        {
            // Arrage: Provide a definition of what wants to be retrieved from the flat file
            DataItemDefinition definition = new DataItemDefinition();

            definition.Properties.Add(new DataItemProperty() { Name = "Title", DataType = typeof(String), OridinalPosition = (hasHeaderRecord ? -1 : 0) });
            definition.Properties.Add(new DataItemProperty() { Name = "CreatedDate", DataType = typeof(DateTime), OridinalPosition = (hasHeaderRecord ? -1 : 1) });
            definition.Properties.Add(new DataItemProperty() { Name = "Size", DataType = typeof(Double), OridinalPosition = (hasHeaderRecord ? -1 : 2) });
            definition.Properties.Add(new DataItemProperty() { Name = "Description", DataType = typeof(String), OridinalPosition = (hasHeaderRecord ? -1 : 3) });
            definition.Properties.Add(new DataItemProperty() { Name = "Active", DataType = typeof(Boolean), OridinalPosition = (hasHeaderRecord ? -1 : 4) });
            definition.HasHeaderRecord = hasHeaderRecord; // No Header Record For This Test

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
