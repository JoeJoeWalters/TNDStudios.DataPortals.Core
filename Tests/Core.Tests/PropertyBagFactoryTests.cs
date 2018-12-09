using System;
using Xunit;
using TNDStudios.DataPortals.Data;
using System.Collections.Generic;
using TNDStudios.DataPortals.PropertyBag;

namespace TNDStudios.DataPortals.Tests.Core
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class PropertyBagFactoryTestsFixture : IDisposable
    {
        public PropertyBagFactory Factory; // The property bag factory to test

        /// <summary>
        /// Configure the test fixture
        /// </summary>
        public PropertyBagFactoryTestsFixture()
        {
            Factory = new PropertyBagFactory();
        }

        /// <summary>
        /// Dispose of the factory class etc.
        /// </summary>
        public void Dispose()
        {
            Factory = null; // Kill the reference
        }
    }

    public class PropertyBagFactoryTests : IClassFixture<PropertyBagFactoryTestsFixture>
    {
        private PropertyBagFactoryTestsFixture fixture; // Reference for the test fixture

        /// <summary>
        /// Constructor to inject the fixture
        /// </summary>
        /// <param name="data"></param>
        public PropertyBagFactoryTests(PropertyBagFactoryTestsFixture data)
            => fixture = data;

        [Fact]
        public void Get_Provider_PropertyBagTypes_From_Factory()
        {
            // Arrange            
            Dictionary<DataProviderType, List<PropertyBagItemType>> results = 
                new Dictionary<DataProviderType, List<PropertyBagItemType>>();
            List<PropertyBagItemType> unknownResult = null;

            // Act
            foreach (DataProviderType type in
                ((DataProviderType[])Enum.GetValues(typeof(DataProviderType))))
            {
                // Get the result from each enumeration
                List<PropertyBagItemType> result =
                    fixture.Factory.Get(ObjectTypes.Connections, (Int32)type);
                if (type == DataProviderType.Unknown)
                    unknownResult = result;
                else
                    results.Add(type, result);
            }

            // Assert
            Assert.Equal(
                Enum.GetValues(typeof(DataProviderType)).Length - 1, 
                results.Count); // Did we get property bags for each provider?

            Assert.True(unknownResult.Count == 0); // Unknown provider should result in an empty set

            foreach(KeyValuePair<DataProviderType, List<PropertyBagItemType>> result in results)
            {
                Assert.NotNull(result.Value); // The provider factory did not result in a null array
                Assert.True(result.Value.Count > 0); // There must be some items in the bag
            }
        }
    }
}
