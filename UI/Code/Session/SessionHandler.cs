using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.PropertyBag;
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
                Guid dataTextConnectionId = Guid.NewGuid();
                Guid dataDefinitionId = Guid.NewGuid();
                Guid apiId = Guid.NewGuid();
                Guid delimitedCredentialsId = Guid.NewGuid();

                Guid dataSqlConnectionId = Guid.NewGuid();
                Guid dataSqlDefinitionId = Guid.NewGuid();
                Guid apiSqlId = Guid.NewGuid();
                Guid sqlCredentialsId = Guid.NewGuid();
                Guid sqlApiCredentialsId = Guid.NewGuid();

                Guid transformationId = Guid.NewGuid();

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
                                Id = delimitedCredentialsId,
                                Name = "Delimited File Credentials",
                                Description = "Delimited File Credentials",
                                Properties = new List<Credential>()
                                {
                                    new Credential(){ Name = "Username", Value = "username" },
                                    new Credential(){ Name = "Password", Value = "password", Encrypted = true },
                                    new Credential(){ Name = "ClientId", Value = "client id" },
                                    new Credential(){ Name = "ClientSecret", Value = "client secret" }
                                }
                            },
                            new Credentials()
                            {
                                Id = sqlCredentialsId,
                                Name = "SQL Credentials",
                                Description = "SQL Credentials",
                                Properties = new List<Credential>()
                                {
                                    new Credential(){ Name = "Username", Value = "TransactionUser" },
                                    new Credential(){ Name = "Password", Value = "password", Encrypted = true }
                                }
                            },
                            new Credentials()
                            {
                                Id = sqlApiCredentialsId,
                                Name = "SQL API Credentials",
                                Description = "SQL API Credentials",
                                Properties = new List<Credential>()
                                {
                                    new Credential(){ Name = "Username", Value = "username" },
                                    new Credential(){ Name = "Password", Value = "password", Encrypted = true }
                                }
                            }
                        },
                        ApiDefinitions = new List<ApiDefinition>()
                        {
                            new ApiDefinition()
                            {
                                DataConnection = dataTextConnectionId,
                                DataDefinition = dataDefinitionId,
                                Description = "Flat File Api Definition",
                                Id = apiId,
                                Name = "flatfile",
                                CredentialsLinks = new List<CredentialsLink>()
                                {
                                    new CredentialsLink()
                                    {
                                        Permissions = new Permissions()
                                        {
                                            CanCreate = true,
                                            CanDelete = true,
                                            CanRead = true,
                                            CanUpdate = true,
                                            Filter = "[Column 3] > 1.5"
                                        },
                                        Credentials = delimitedCredentialsId
                                    }
                                }
                            },
                            new ApiDefinition()
                            {
                                DataConnection = dataSqlConnectionId,
                                DataDefinition = dataSqlDefinitionId,
                                Description = "SQL Api Definition",
                                Id = apiSqlId,
                                Name = "sql",
                                CredentialsLinks = new List<CredentialsLink>()
                                {
                                    new CredentialsLink()
                                    {
                                        Permissions = new Permissions()
                                        {
                                            CanCreate = true,
                                            CanDelete = true,
                                            CanRead = true,
                                            CanUpdate = true,
                                            Filter = "TR_ApiVersion > 1.0"
                                        },
                                        Credentials = sqlApiCredentialsId
                                    }
                                }
                            }
                        },
                        DataConnections = new List<DataConnection>()
                        {
                            new DataConnection()
                            {
                                Id = dataTextConnectionId,
                                ConnectionString = @"C:\Users\Joe\Documents\Git\TNDStudios.DataPortals.Core\Tests\Providers\DelimitedFileProvider.Tests\TestFiles\DataTypesTest.txt",
                                Description = "Delimited Data Connection Description",
                                Name = "Delimited Data Connection",
                                ProviderType = DataProviderType.DelimitedFileProvider,
                                Credentials = delimitedCredentialsId,
                                PropertyBag = (new PropertyBagFactory())
                                    .Get(ObjectTypes.Connections, (Int32)DataProviderType.DelimitedFileProvider)
                                    .Select(type =>
                                        new PropertyBagItem()
                                        {
                                            Value = type.DefaultValue,
                                            ItemType = type
                                        })
                                    .ToList()
                            },
                            new DataConnection()
                            {
                                Id = dataSqlConnectionId,
                                ConnectionString = @"Server=.;Database=TransactionRepository;User Id={{Username}};Password={{Password}}",
                                Description = "SQL Data Connection Description",
                                Name = "SQL Data Connection",
                                ProviderType = DataProviderType.SQLProvider,
                                Credentials = sqlCredentialsId,
                                PropertyBag = (new PropertyBagFactory())
                                    .Get(ObjectTypes.Connections, (Int32)DataProviderType.SQLProvider)
                                    .Select(type =>
                                        new PropertyBagItem()
                                        {
                                            Value = type.DefaultValue,
                                            ItemType = type
                                        })
                                    .ToList(),
                                ObjectName = "TransactionRepository.dbo.Workers"
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
                                        DataType = typeof(Double),
                                        Name = "Column 3",
                                        Description = "Column 3",
                                        Path = "",
                                        PropertyType = DataItemPropertyType.Property,
                                        OrdinalPosition = 2,
                                        Quoted = false
                                    }
                                },
                                Name = "Data Item Definition",
                                PropertyBag = new List<PropertyBagItem>()
                                {
                                }
                            },
                            new DataItemDefinition()
                            {
                                Id = dataSqlDefinitionId,
                                Culture = CultureInfo.GetCultureInfo("en-GB"),
                                Description = "SQL Data Item Definition Description",
                                EncodingFormat = Encoding.UTF8,
                                ItemProperties = new List<DataItemProperty>()
                                {
                                    new DataItemProperty()
                                    {
                                        DataType = typeof(Int64),
                                        Name = "TR_PkId",
                                        Description = "TR_PkId",
                                        Path = "TR_PkId",
                                        PropertyType = DataItemPropertyType.Property,
                                        OrdinalPosition = 0,
                                        Quoted = false,
                                        Key = true
                                    },
                                    new DataItemProperty()
                                    {
                                        DataType = typeof(Decimal),
                                        Name = "TR_ApiVersion",
                                        Description = "TR_ApiVersion",
                                        Path = "TR_ApiVersion",
                                        PropertyType = DataItemPropertyType.Property,
                                        OrdinalPosition = 1,
                                        Quoted = false
                                    },
                                    new DataItemProperty()
                                    {
                                        DataType = typeof(DateTime),
                                        Name = "TR_CreatedDateTime",
                                        Description = "TR_CreatedDateTime",
                                        Path = "TR_CreatedDateTime",
                                        PropertyType = DataItemPropertyType.Property,
                                        OrdinalPosition = 2,
                                        Quoted = false,
                                        Key = true
                                    },
                                    new DataItemProperty()
                                    {
                                        DataType = typeof(String),
                                        Name = "Email",
                                        Description = "Email",
                                        Path = "Email",
                                        PropertyType = DataItemPropertyType.Property,
                                        OrdinalPosition = 8,
                                        Quoted = false,
                                        Key = true
                                    },
                                    new DataItemProperty()
                                    {
                                        DataType = typeof(String),
                                        Name = "Name",
                                        Description = "Name",
                                        Path = "Name",
                                        PropertyType = DataItemPropertyType.Property,
                                        OrdinalPosition = 8,
                                        Quoted = false,
                                        Key = true
                                    }
                                },
                                Name = "SQL Data Item Definition",
                                PropertyBag = new List<PropertyBagItem>()
                                {
                                }
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
