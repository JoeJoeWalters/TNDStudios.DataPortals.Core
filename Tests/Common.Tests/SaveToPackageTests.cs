using System;
using Xunit;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Helpers;
using System.Collections.Generic;
using TNDStudios.DataPortals.Repositories;
using TNDStudios.DataPortals.Security;
using TNDStudios.DataPortals.Api;
using System.Globalization;
using System.Text;

namespace TNDStudios.DataPortals.Tests.Common
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class SaveToPackageTestsFixture : IDisposable
    {
        public Package Package; // The package to test

        /// <summary>
        /// Configure the test fixture
        /// </summary>
        public SaveToPackageTestsFixture()
            => Initialise();

        public void Initialise()
            => Package = new Package()
            {
                Id = Guid.NewGuid(),
                Name = "Test Package",
                Description = "Test Package Description"
            };

        /// <summary>
        /// Dispose of the repository class etc.
        /// </summary>
        public void Dispose()
            => this.Package = null; // Kill the reference
    }

    public class SaveToPackageTests : IClassFixture<SaveToPackageTestsFixture>
    {
        private SaveToPackageTestsFixture fixture; // Reference for the test fixture

        /// <summary>
        /// Constructor to inject the fixture
        /// </summary>
        /// <param name="data"></param>
        public SaveToPackageTests(SaveToPackageTestsFixture data)
            => fixture = data;

        /// <summary>
        /// Test saving a connection in to a package
        /// </summary>
        [Fact]
        public void Save_Connection_To_Package()
        {
            // Arrange
            fixture.Initialise(); // Cleanse the fixture and reset it
            DataConnection toSave = new DataConnection()
            {
                ConnectionString = "Connection String",
                Credentials = new Guid(),
                Description = "Test Connection Description",
                Name = "Test Connection",
                ObjectName = "Object Name",
                ProviderType = DataProviderType.DelimitedFileProvider
            };
            DataConnection afterSave = null;

            // Act
            afterSave = fixture.Package.Save<DataConnection>(toSave);

            // Assert
            Assert.NotEqual(Guid.Empty, afterSave.Id);
            Assert.Equal(fixture.Package, afterSave.ParentPackage);
        }

        /// <summary>
        /// Test saving a data definition to a package
        /// </summary>
        [Fact]
        public void Save_DataDefinition_To_Package()
        {
            // Arrange
            fixture.Initialise(); // Cleanse the fixture and reset it
            DataItemDefinition toSave = new DataItemDefinition()
            {
                Culture = new CultureInfo("en-GB"),
                EncodingFormat = Encoding.UTF8,
                Description = "Test Data Definition Description",
                ItemProperties = new List<DataItemProperty>()
                {
                    new DataItemProperty()
                    {
                        Calculation = "Calculation",
                        DataType = typeof(String),
                        Description = "Item Description",
                        Name = "Item Name",
                        Key = false,
                        OrdinalPosition = 0,
                        Path = "Path",
                        Pattern = "Pattern",
                        PropertyType = DataItemPropertyType.Property,
                        Quoted = false,
                        Size = 50
                    }
                },
                Name = "Test Data Definition"
            };
            DataItemDefinition afterSave = null;

            // Act
            afterSave = fixture.Package.Save<DataItemDefinition>(toSave);

            // Assert
            Assert.NotEqual(Guid.Empty, afterSave.Id);
            Assert.Equal(fixture.Package, afterSave.ParentPackage);
        }

        /// <summary>
        /// Test saving a set of credentails to a package
        /// </summary>
        [Fact]
        public void Save_Credentials_To_Package()
        {
            // Arrange
            fixture.Initialise(); // Cleanse the fixture and reset it
            Credentials toSave = new Credentials()
            {
                 Description = "Test Credentials Description",
                  Name = "Test Credentials",
                   Properties = new List<Credential>()
                   {
                       new Credential()
                       {
                            Description = "Test Credential Description",
                            Encrypted = true,
                            Name = "Test Credential",
                            Value = "Test Credenital Value"
                       }
                   }
            };
            Credentials afterSave = null;

            // Act
            afterSave = fixture.Package.Save<Credentials>(toSave);

            // Assert
            Assert.NotEqual(Guid.Empty, afterSave.Id);
            Assert.Equal(fixture.Package, afterSave.ParentPackage);
        }

        /// <summary>
        /// Test saving a transformation to a package
        /// </summary>
        [Fact]
        public void Save_Transformation_To_Package()
        {
            // Arrange
            fixture.Initialise(); // Cleanse the fixture and reset it
            Transformation toSave = new Transformation()
            {
                Description = "Test Transformation Description",
                Destination = Guid.NewGuid(),
                Name = "Test Transformation",
                Source = Guid.NewGuid()
            };
            Transformation afterSave = null;

            // Act
            afterSave = fixture.Package.Save<Transformation>(toSave);

            // Assert
            Assert.NotEqual(Guid.Empty, afterSave.Id);
            Assert.Equal(fixture.Package, afterSave.ParentPackage);
        }

        /// <summary>
        /// Test saving a managed api to a package
        /// </summary>
        [Fact]
        public void Save_Api_To_Package()
        {
            // Arrange
            fixture.Initialise(); // Cleanse the fixture and reset it
            ApiDefinition toSave = new ApiDefinition()
            {
                 Aliases = new List<KeyValuePair<String, String>>()
                 {
                     new KeyValuePair<String, String>("Search", "Replacement")
                 },
                CredentialsLinks = new List<CredentialsLink>()
                {
                    new CredentialsLink()
                    {
                        Credentials = new Guid(),
                        Permissions = new Permissions()
                        {
                        CanCreate = true,
                            CanDelete = true,
                            CanRead = true,
                            CanUpdate = true,
                            Filter = "Filter = true"
                        }
                    }
                },
                DataConnection = new Guid(),
                DataDefinition = new Guid(),
                Description = "Test Api Description",
                Name = "Test Api"                     
            };
            ApiDefinition afterSave = null;

            // Act
            afterSave = fixture.Package.Save<ApiDefinition>(toSave);

            // Assert
            Assert.NotEqual(Guid.Empty, afterSave.Id);
            Assert.Equal(fixture.Package, afterSave.ParentPackage);
        }
    }
}
