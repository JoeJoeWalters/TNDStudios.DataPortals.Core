using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.DataPortals.UI.Models.Api;
using Xunit;

namespace TNDStudios.DataPortals.Tests.UI
{
    /// <summary>
    /// Tests for the data item definition model class
    /// </summary>
    public class DataItemDefinitionModelTests
    {

        /// <summary>
        /// Test to see if a given data definition is valid
        /// </summary>
        [Fact]
        public void Is_DataDefinition_Valid()
        {
            // Arrange
            DataItemDefinitionModel dataItemDefinition =
                new DataItemDefinitionModel()
                {
                    EncodingFormat = "UTF8",
                    Culture = "en-GB",
                    ItemProperties = 
                        new List<DataItemPropertyModel>()
                        {
                            new DataItemPropertyModel()
                            {
                                
                            } // No need to set any values, just that there are some
                        }
                }; // A data item definition with no properties

            // Act

            // Assert
            Assert.True(dataItemDefinition.IsValid); // Is this model valid?
        }

        /// <summary>
        /// Test to see if a given data definition is not valid
        /// </summary>
        [Fact]
        public void Is_DataDefinition_Not_Valid()
        {
            // Arrange
            DataItemDefinitionModel dataItemDefinition =
                new DataItemDefinitionModel()
                {
                    EncodingFormat = String.Empty,
                    Culture = String.Empty,
                    ItemProperties = new List<DataItemPropertyModel>() { }
                }; // A data item definition with no properties

            // Act

            // Assert
            Assert.False(dataItemDefinition.IsValid); // Is this model not valid?
        }
    }
}
