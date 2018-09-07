using System;
using Xunit;
using System.Reflection;
using System.IO;

namespace TNDStudios.DataPortals.Tests.FlatFileProvider
{
    public class Tests
    {
        [Fact]
        public void Test_Test()
        {
            // Arrange
            Stream resourceStream = GetResourceStream("TestFiles.CSVTest.txt");

            // Act

            // Assert
            Assert.True(resourceStream != null);
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
