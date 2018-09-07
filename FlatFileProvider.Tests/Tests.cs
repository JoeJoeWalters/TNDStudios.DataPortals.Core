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
            DataItemDefinition definition = new DataItemDefinition();
            definition.Properties.Add(new DataItemProperty() { Name = "Position 0", DataType = typeof(String), OridinalPosition = 0 });
            definition.Properties.Add(new DataItemProperty() { Name = "Position 1", DataType = typeof(DateTime), OridinalPosition = 1 });
            definition.Properties.Add(new DataItemProperty() { Name = "Position 2", DataType = typeof(Double), OridinalPosition = 2 });

            // Act
            DataTable data = provider.ExecuteReader(definition, "");

            // Assert
            Assert.True(data.Rows.Count != 0);
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
