using System;
using Xunit;
using AutoMapper;
using TNDStudios.DataPortals.UI;
using TNDStudios.DataPortals.UI.Models;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.UI.Models.Api;
using System.Globalization;
using System.Text;

namespace TNDStudios.DataPortals.Tests.UI
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class AutoMapperTestsFixture : IDisposable
    {
        public IMapper TestMapper; // Mapper to use for the test fixture

        /// <summary>
        /// Configure the test fixture
        /// </summary>
        public AutoMapperTestsFixture()
        {
            // Initialise the mapper with the correct namespace
            Mapper.Reset(); // Only used for tests, clear other mappings
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfiles("TNDStudios.DataPortals.UI");
            });

            // Create the test mapper
            TestMapper = new Mapper(Mapper.Configuration);
        }

        /// <summary>
        /// Dispose of the mapper and the global mappings
        /// </summary>
        public void Dispose()
        {
            TestMapper = null;
            Mapper.Reset();
        }
    }

    public class AutoMapperTests : IClassFixture<AutoMapperTestsFixture>
    {
        private AutoMapperTestsFixture fixture; // Reference for the test fixture

        /// <summary>
        /// Constructor to inject the fixture
        /// </summary>
        /// <param name="data"></param>
        public AutoMapperTests(AutoMapperTestsFixture data)
            => fixture = data;

        /// <summary>
        /// Test the Data Item Definition class transformed to the API Model
        /// </summary>
        [Fact]
        public void DataItemDefinition_To_Model()
        {
            String culture = "en-GB";
            String encoding = "utf-8";
            DataItemDefinition definition = new DataItemDefinition()
            {
                Culture = CultureInfo.GetCultureInfo(culture),
                EncodingFormat = Encoding.GetEncoding(encoding)
            };

            // Act
            DataItemDefinitionModel model =
                fixture.TestMapper.Map<DataItemDefinitionModel>(definition);

            // Assert
            Assert.Equal(definition.ItemProperties.Count, model.ItemProperties.Count);
            Assert.Equal(culture, model.Culture);
            Assert.Equal(encoding, model.EncodingFormat);
        }

        /// <summary>
        /// Test the Data Item Property class transformed to the API Model
        /// </summary>
        [Fact]
        public void DataItemProperty_To_Model()
        {

        }

        /// <summary>
        /// Test the Data Item Definition Model transformed to the Domain object
        /// </summary>
        [Fact]
        public void DataItemDefinitionModel_To_Domain()
        {
            String culture = "en-GB";
            String encoding = "utf-8";
            DataItemDefinitionModel model = new DataItemDefinitionModel()
            {
                Culture = culture,
                EncodingFormat = encoding
            };

            // Act
            DataItemDefinition definition =
                fixture.TestMapper.Map<DataItemDefinition>(model);

            // Assert
            Assert.Equal(definition.ItemProperties.Count, model.ItemProperties.Count);
            Assert.Equal(culture, model.Culture);
            Assert.Equal(encoding, model.EncodingFormat);
        }

        /// <summary>
        /// Test the Data Item Property Model transformed to the Domain object
        /// </summary>
        [Fact]
        public void DataItemPropertyModel_To_Domain()
        {

        }
    }
}
