using System;
using Xunit;
using AutoMapper;
using TNDStudios.DataPortals.UI;
using TNDStudios.DataPortals.UI.Models;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.Tests.UI
{
    public class AutoMapperTests
    {

        public AutoMapperTests()
        {

        }

        /// <summary>
        /// Test the Data Item Definition class transformed to the API Model
        /// </summary>
        [Fact]
        public void DataItemDefinition_To_Model()
        {
            // Arrange
            Mapper.Initialize(cfg => 
            {
                cfg.AddProfiles("TNDStudios.DataPortals.UI");
            });

            IMapper mapper = new Mapper(Mapper.Configuration);
            DataItemDefinition definition = new DataItemDefinition()
            {

            };

            // Act

            // Assert
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
