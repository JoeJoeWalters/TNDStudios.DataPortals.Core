using System;
using Xunit;
using System.Reflection;
using System.IO;

namespace TNDStudios.DataPortals.Data.FlatFileProvider
{
    public class Tests
    {
        /// <summary>
        /// Effective constants or read only values
        /// </summary>
        public readonly String NamespacePrefix = "FlatFileProvider.Tests.";

        [Fact]
        public void Test_Test()
        {
            // Arrange
            Stream resourceStream = GetResourceStream($"{NamespacePrefix}CSVTest.txt");

            // Act

            // Assert

        }

        /// <summary>
        /// Build a memory stream from the embedded resource to feed to the test scenarios
        /// </summary>
        /// <param name="embeddedResourceName">The name of the resource to read</param>
        /// <returns>A memory stream with the data contained within</returns>
        private Stream GetResourceStream(String embeddedResourceName)
            => Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceName);
    }
}
