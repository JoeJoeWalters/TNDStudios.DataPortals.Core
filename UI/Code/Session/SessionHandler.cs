using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Repositories;
using TNDStudios.DataPortals.Security;

namespace TNDStudios.DataPortals.UI
{
    /// <summary>
    /// Static class to handle the session items
    /// </summary>
    public static class SessionHandler
    {
        /// <summary>
        /// If the session handler has been initialised
        /// </summary>
        public static Boolean Initialised { get; set; }

        /// <summary>
        /// Static package to hold the activity for the website
        /// </summary>
        public static IPackageRepository PackageRepository { get; set; }

        /// <summary>
        /// Initialise the session handler
        /// </summary>
        public static void Initialise()
        {
            // Initialised?
            if (!Initialised)
            {
                // For testing / development for now just use a memory implementation
                // of the package repository
                PackageRepository = new MemoryPackageRepository();

                // Initialisation Id's
                Guid dataConnectionId = Guid.NewGuid();
                Guid dataDefinitionId = Guid.NewGuid();
                Guid apiId = Guid.NewGuid();
                Guid transformationId = Guid.NewGuid();
                Guid CredentialsId = Guid.NewGuid();

                // Set up a new test package in the repository
                PackageRepository.Save(
                    new Package()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Test Package",
                        Description = "Test Package Description",
                        CredentialsStore = new List<Credentials>()
                        {
                            new Credentials()
                            {
                                Id = CredentialsId,
                                Name = "Default Credentials",
                                Description = "Credentials Description",
                                Properties = new List<Credential>()
                                {
                                    new Credential(){ Name = "Username", Value = "username" },
                                    new Credential(){ Name = "Password", Value = "password", Encrypted = true },
                                    new Credential(){ Name = "ClientId", Value = "client id" },
                                    new Credential(){ Name = "ClientSecret", Value = "client secret" }
                                }
                            }
                        },
                        ApiDefinitions = new List<ApiDefinition>()
                        {
                            new ApiDefinition()
                            {
                                DataConnection = dataConnectionId,
                                DataDefinition = dataDefinitionId,
                                Description = "Flat File Api Definition",
                                Id = apiId,
                                Name = "flatfile"
                            }
                        },
                        DataConnections = new List<DataConnection>()
                        {
                            new DataConnection()
                            {
                                Id = dataConnectionId,
                                ConnectionString = @"C:\Users\Joe\Documents\Git\TNDStudios.DataPortals.Core\Tests\Providers\DelimitedFileProvider.Tests\TestFiles\DataTypesTest.txt",
                                Description = "Data Connection Description",
                                Name = "Data Connection",
                                ProviderType = DataProviderType.DelimitedFileProvider
                            }
                        },
                        DataDefinitions = new List<DataItemDefinition>()
                        {
                            new DataItemDefinition()
                            {
                                Id = dataDefinitionId,
                                Culture = CultureInfo.GetCultureInfo("en-GB"),
                                Description = "Data Item Definition Description",
                                EncodingFormat = Encoding.UTF8,
                                ItemProperties = new List<DataItemProperty>()
                                {
                                    new DataItemProperty()
                                    {
                                        DataType = typeof(String),
                                        Name = "Column 1",
                                        Description = "Column 1",
                                        Path = "",
                                        PropertyType = DataItemPropertyType.Property,
                                        OrdinalPosition = 0,
                                        Quoted = false
                                    },
                                    new DataItemProperty()
                                    {
                                        DataType = typeof(DateTime),
                                        Name = "Column 2",
                                        Description = "Column 2",
                                        Path = "",
                                        PropertyType = DataItemPropertyType.Property,
                                        OrdinalPosition = 1,
                                        Quoted = false
                                    },
                                    new DataItemProperty()
                                    {
                                        DataType = typeof(String),
                                        Name = "Column 3",
                                        Description = "Column 3",
                                        Path = "",
                                        PropertyType = DataItemPropertyType.Property,
                                        OrdinalPosition = 2,
                                        Quoted = false
                                    }
                                },
                                Name = "Data Item Definition",
                                PropertyBag = new Dictionary<String, Object>()
                            }
                        },
                        Transformations = new List<Transformation>()
                        {
                            new Transformation()
                            {
                                Id = transformationId,
                                Name = "Transformation",
                                Description = "Transformation Description",
                            }
                        }
                    });

                // Now intialised
                Initialised = true;
            }
        }
    }
}
