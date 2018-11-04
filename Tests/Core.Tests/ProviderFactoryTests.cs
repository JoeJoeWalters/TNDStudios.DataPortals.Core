using System;
using Xunit;
using TNDStudios.DataPortals.Data;
using System.Collections.Generic;

namespace TNDStudios.DataPortals.Tests.Core
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class ProviderFactoryTestsFixture : IDisposable
    {
        public DataProviderFactory Factory; // The provider factory to test

        /// <summary>
        /// Configure the test fixture
        /// </summary>
        public ProviderFactoryTestsFixture()
        {
            Factory = new DataProviderFactory();
        }

        /// <summary>
        /// Dispose of the factory class etc.
        /// </summary>
        public void Dispose()
        {
            Factory = null; // Kill the reference
        }
    }

    public class ProviderFactoryTests : IClassFixture<ProviderFactoryTestsFixture>
    {
        private ProviderFactoryTestsFixture fixture; // Reference for the test fixture

        /// <summary>
        /// Constructor to inject the fixture
        /// </summary>
        /// <param name="data"></param>
        public ProviderFactoryTests(ProviderFactoryTestsFixture data)
            => fixture = data;

        [Fact]
        public void Get_Providers_From_Factory()
        {
            // Arrange
            List<KeyValuePair<DataProviderType, Type>>
                expectedOutcomes = new List<KeyValuePair<DataProviderType, Type>>()
                {
                    new KeyValuePair<DataProviderType, Type>(DataProviderType.DelimitedFileProvider, typeof(DelimitedFileProvider)),
                    new KeyValuePair<DataProviderType, Type>(DataProviderType.FixedWidthFileProvider, typeof(FixedWidthFileProvider)),
                    new KeyValuePair<DataProviderType, Type>(DataProviderType.SQLProvider, typeof(SQLProvider)),
                    new KeyValuePair<DataProviderType, Type>(DataProviderType.Unknown, null)
                };
            DataProviderFactory factory = new DataProviderFactory();

            // Assert & Act
            expectedOutcomes.ForEach(outcome =>
            {
                // Get the provider based on the provider type
                IDataProvider provider = factory.Get(
                    new DataConnection()
                    {
                        ProviderType = outcome.Key
                    }, false);

                // Is it the right type?
                if (outcome.Key == DataProviderType.Unknown)
                    Assert.Null(provider);
                else
                    Assert.True(outcome.Value == provider.GetType()); // Can't use Assert.Type as it's a runtime type
            });
        }
    }
}
