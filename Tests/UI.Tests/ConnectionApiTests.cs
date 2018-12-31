using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Repositories;
using TNDStudios.DataPortals.Security;
using TNDStudios.DataPortals.UI;
using TNDStudios.DataPortals.UI.Controllers.Api.Helpers;
using TNDStudios.DataPortals.UI.Models.Api;
using Xunit;

namespace TNDStudios.DataPortals.Tests.UI
{
    /// <summary>
    /// Tests for the connection api controller and it's associated
    /// helper classes
    /// </summary>
    public class ConnectionApiTests
    {
        [Fact]
        public void Populate_ConnectionModel()
        {
            // Arrange
            Guid credentialsId = Guid.NewGuid(); // Set the fixed id of the credentials

            // Create a new package to pick the credentials up from in the populate routine
            Package package = new Package()
            {
                CredentialsStore = new List<Credentials>()
                {
                    new Credentials(){ Id = credentialsId, Name = "Dummy Credentials" }
                }
            };

            // Create a new connection model without the populated fields
            DataConnectionModel model = new DataConnectionModel()
            {
                Credentials = 
                    new KeyValuePair<Guid, string>
                        (credentialsId, String.Empty),
                ProviderType = (Int32)DataProviderType.SQLProvider
            };

            // Set up the mapper to map any needed objects
            AutoMapperProfile.Initialise();
            IMapper mapper = new Mapper(Mapper.Configuration);

            // Act
            model = (new ConnectionApiHelpers())
                .PopulateModel(mapper, package, model);

            // Assert
            Assert.True(model.Credentials.Value != String.Empty);
            Assert.True(model.ProviderData != null);
        }
    }
}
