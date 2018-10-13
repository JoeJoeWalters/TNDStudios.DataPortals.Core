using System;
using Xunit;
using AutoMapper;
using TNDStudios.DataPortals.UI;
using TNDStudios.DataPortals.UI.Models;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.UI.Models.Api;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using TNDStudios.DataPortals.Api;

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
        /// Test the linking of Guid key's from one object to the other 
        /// with a name value being brought along for population
        /// (Used when foreign keys to other objects are needed without
        /// problems caused by infinite loops with embedded byref objects)
        /// </summary>
        [Fact]
        public void Guid_To_GuidKeyValuePair()
        {
            // Arrange
            List<Guid> keys = new List<Guid>()
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            // Act
            List<KeyValuePair<Guid, String>> keyValuePairs =
                fixture.TestMapper.Map<List<KeyValuePair<Guid, String>>>(keys);

            // Assert
            Assert.Equal(keys.Count, keyValuePairs.Count);
            for (Int32 itemId = 0; itemId < keys.Count; itemId++)
                Assert.Equal(keys[itemId], keyValuePairs[itemId].Key);
        }

        /// <summary>
        /// Test the linking of Guid key's from one object to the other 
        /// with a name value being brought along for population
        /// (Used when foreign keys to other objects are needed without
        /// problems caused by infinite loops with embedded byref objects)
        /// </summary>
        [Fact]
        public void GuidKeyValuePair_To_Guid()
        {
            // Arrange
            List<KeyValuePair<Guid, String>> keyValuePairs = new List<KeyValuePair<Guid, String>>()
            {
                new KeyValuePair<Guid, String>(Guid.NewGuid(), ""),
                new KeyValuePair<Guid, String>(Guid.NewGuid(), ""),
                new KeyValuePair<Guid, String>(Guid.NewGuid(), "")
            };

            // Act
            List<Guid> keys =
                fixture.TestMapper.Map<List<Guid>>(keyValuePairs);

            // Assert
            Assert.Equal(keys.Count, keyValuePairs.Count);
            for (Int32 itemId = 0; itemId < keys.Count; itemId ++)
                Assert.Equal(keys[itemId], keyValuePairs[itemId].Key);
        }

        /// <summary>
        /// Test the Data Connection class transformed to the API Model
        /// </summary>
        [Fact]
        public void DataConnection_To_Model()
        {
            // Arrange
            DataConnection connection = new DataConnection()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ConnectionString = "Connection String Content",
                Definitions = new List<Guid>()
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid()
                },
                ProviderType = DataProviderType.MSSQLProvider
            };

            // Act
            DataConnectionModel model =
                fixture.TestMapper.Map<DataConnectionModel>(connection);

            // Assert
            Assert.Equal(connection.Id, model.Id);
            Assert.Equal(connection.Name, model.Name);
            Assert.Equal(connection.Description, model.Description);
            Assert.Equal(connection.ConnectionString, model.ConnectionString);
            Assert.Equal(connection.Definitions.Count, model.Definitions.Count);
            for (Int32 itemId = 0; itemId < connection.Definitions.Count; itemId++)
                Assert.Equal(connection.Definitions[itemId], model.Definitions[itemId].Key);
            Assert.Equal(connection.ProviderType, (DataProviderType)model.ProviderType);
        }

        /// <summary>
        /// Test the Data Connection Model transformed to the Domain object
        /// </summary>
        [Fact]
        public void DataConnectionModel_To_Domain()
        {
            // Arrange
            DataConnectionModel model = new DataConnectionModel()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ConnectionString = "Connection String Content",
                Definitions = new List<KeyValuePair<Guid, String>>()
                {
                    new KeyValuePair<Guid, String>(Guid.NewGuid(), "Item 1"),
                    new KeyValuePair<Guid, String>(Guid.NewGuid(), "Item 2"),
                    new KeyValuePair<Guid, String>(Guid.NewGuid(), "Item 3")
                },
                ProviderType = (Int32)DataProviderType.MSSQLProvider
            };

            // Act
            DataConnection connection =
                fixture.TestMapper.Map<DataConnection>(model);

            // Assert
            Assert.Equal(connection.Id, model.Id);
            Assert.Equal(connection.Name, model.Name);
            Assert.Equal(connection.Description, model.Description);
            Assert.Equal(connection.ConnectionString, model.ConnectionString);
            Assert.Equal(connection.Definitions.Count, model.Definitions.Count);
            for (Int32 itemId = 0; itemId < connection.Definitions.Count; itemId++)
                Assert.Equal(connection.Definitions[itemId], model.Definitions[itemId].Key);
            Assert.Equal(connection.ProviderType, (DataProviderType)model.ProviderType);
        }

        /// <summary>
        /// Test the API Definition class transformed to the API Model
        /// </summary>
        [Fact]
        public void ApiDefinition_To_Model()
        {
            // Arrange
            ApiDefinition definition = new ApiDefinition()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                DataConnection = Guid.NewGuid(),
                DataDefinition = Guid.NewGuid()
            };

            // Act
            ApiDefinitionModel model =
                fixture.TestMapper.Map<ApiDefinitionModel>(definition);

            // Assert
            Assert.Equal(definition.Id, model.Id);
            Assert.Equal(definition.Name, model.Name);
            Assert.Equal(definition.Description, model.Description);
            Assert.Equal(definition.DataConnection, model.DataConnection.Key);
            Assert.Equal(definition.DataDefinition, model.DataDefinition.Key);

        }

        /// <summary>
        /// Test the API Definition Model transformed to the Domain object
        /// </summary>
        [Fact]
        public void ApiDefinitionModel_To_Domain()
        {
            // Arrange
            ApiDefinitionModel model = new ApiDefinitionModel()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                DataConnection = new KeyValuePair<Guid, String>(Guid.NewGuid(), "Connection"),
                DataDefinition = new KeyValuePair<Guid, String>(Guid.NewGuid(), "Definition")
            };

            // Act
            ApiDefinition definition =
                fixture.TestMapper.Map<ApiDefinition>(model);

            // Assert
            Assert.Equal(definition.Id, model.Id);
            Assert.Equal(definition.Name, model.Name);
            Assert.Equal(definition.Description, model.Description);
            Assert.Equal(definition.DataConnection, model.DataConnection.Key);
            Assert.Equal(definition.DataDefinition, model.DataDefinition.Key);
        }

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
                Name = "Name",
                Description = "Description",
                Culture = CultureInfo.GetCultureInfo(culture),
                EncodingFormat = Encoding.GetEncoding(encoding),
                Connections = new List<Guid>()
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid()
                }
            };

            // Act
            DataItemDefinitionModel model =
                fixture.TestMapper.Map<DataItemDefinitionModel>(definition);

            // Assert
            Assert.Equal(definition.ItemProperties.Count, model.ItemProperties.Count);
            Assert.Equal(culture, model.Culture);
            Assert.Equal(encoding, model.EncodingFormat);
            Assert.Equal(definition.Id, model.Id);
            Assert.Equal(definition.Name, model.Name);
            Assert.Equal(definition.Description, model.Description);
            Assert.Equal(definition.Connections.Count, model.Connections.Count);
            for (Int32 itemId = 0; itemId < definition.Connections.Count; itemId++)
                Assert.Equal(model.Connections[itemId].Key, definition.Connections[itemId]);
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
                Id = Guid.NewGuid(),
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
            Assert.Equal(property.Id, model.Id);
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
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Culture = culture,
                EncodingFormat = encoding,
                Connections = new List<KeyValuePair<Guid, String>>()
                {
                    new KeyValuePair<Guid, string>(Guid.NewGuid(), "Item 1"),
                    new KeyValuePair<Guid, string>(Guid.NewGuid(), "Item 2"),
                    new KeyValuePair<Guid, string>(Guid.NewGuid(), "Item 3")
                }
            };

            // Act
            DataItemDefinition definition =
                fixture.TestMapper.Map<DataItemDefinition>(model);

            // Assert
            Assert.Equal(definition.ItemProperties.Count, model.ItemProperties.Count);
            Assert.Equal(culture, model.Culture);
            Assert.Equal(encoding, model.EncodingFormat);
            Assert.Equal(definition.Id, model.Id);
            Assert.Equal(definition.Name, model.Name);
            Assert.Equal(definition.Description, model.Description);
            Assert.Equal(definition.Connections.Count, model.Connections.Count);
            for (Int32 itemId = 0; itemId < definition.Connections.Count; itemId++)
                Assert.Equal(model.Connections[itemId].Key, definition.Connections[itemId]);
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
