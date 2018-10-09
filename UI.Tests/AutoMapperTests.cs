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
                Id = Guid.NewGuid(),
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
            Assert.Equal(definition.Id.ToString(), model.Id);
        }

        /// <summary>
        /// Test the Data Item Property class transformed to the API Model
        /// </summary>
        [Fact]
        public void DataItemProperty_To_Model()
        {
            String dataType = "System.Int64";
            DataItemProperty property = new DataItemProperty()
            {
                Calculation = "calculated value",
                DataType = Type.GetType(dataType),
                Description = "description value",
                Key = true,
                Name = "name value",
                OridinalPosition = 100001,
                Path = "path/to/the/value",
                Pattern = "dd MMM yyyy",
                PropertyType = DataItemPropertyType.Calculated,
                Quoted = true
            };

            // Act
            DataItemPropertyModel model =
                fixture.TestMapper.Map<DataItemPropertyModel>(property);

            // Assert
            Assert.Equal(property.Calculation, model.Calculation);
            Assert.Equal(dataType, model.DataType);
            Assert.Equal(property.Description, model.Description);
            Assert.Equal(property.Key, model.Key);
            Assert.Equal(property.Name, model.Name);
            Assert.Equal(property.OridinalPosition, model.OridinalPosition);
            Assert.Equal(property.Path, model.Path);
            Assert.Equal(property.Pattern, model.Pattern);
            Assert.Equal(property.PropertyType, model.PropertyType);
            Assert.Equal(property.Quoted, model.Quoted);
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
                Id = Guid.NewGuid().ToString(),
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
            Assert.Equal(definition.Id.ToString(), model.Id);
        }

        /// <summary>
        /// Test the Data Item Property Model transformed to the Domain object
        /// </summary>
        [Fact]
        public void DataItemPropertyModel_To_Domain()
        {
            String dataType = "System.Int64";
            DataItemPropertyModel model = new DataItemPropertyModel()
            {
                Calculation = "calculated value",
                DataType = dataType,
                Description = "description value",
                Key = true,
                Name = "name value",
                OridinalPosition = 100001,
                Path = "path/to/the/value",
                Pattern = "dd MMM yyyy",
                PropertyType = DataItemPropertyType.Calculated,
                Quoted = true
            };

            // Act
            DataItemProperty property =
                fixture.TestMapper.Map<DataItemProperty>(model);

            // Assert
            Assert.Equal(model.Calculation, property.Calculation);
            Assert.Equal(Type.GetType(dataType), property.DataType);
            Assert.Equal(model.Description, property.Description);
            Assert.Equal(model.Key, property.Key);
            Assert.Equal(model.Name, property.Name);
            Assert.Equal(model.OridinalPosition, property.OridinalPosition);
            Assert.Equal(model.Path, property.Path);
            Assert.Equal(model.Pattern, property.Pattern);
            Assert.Equal(model.PropertyType, property.PropertyType);
            Assert.Equal(model.Quoted, property.Quoted);
        }
    }
}
