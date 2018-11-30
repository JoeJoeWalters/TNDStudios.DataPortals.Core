﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Repositories;

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

                // Set up a new test package in the repository
                PackageRepository.Save(
                    new Package()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Test Package",
                        Description = "Test Package Description",
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
                                ConnectionString = @"C:\Users\Joe\Documents\Git\TNDStudios.DataPortals.Core\Tests\Providers\DelimitedFileProvider.Tests\TestFiles\BigFiles\SalesRecords5000.csv",
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
                                Culture = CultureInfo.GetCultureInfo("en-US"),
                                Description = "Data Item Definition Description",
                                EncodingFormat = Encoding.UTF8,
                                ItemProperties = new List<DataItemProperty>()
                                {
                                    new DataItemProperty()
                                    {
                                        DataType = typeof(String),
                                        Name = "Country",
                                        Description = "Country",
                                        Path = "Country",
                                        PropertyType = DataItemPropertyType.Property,
                                        OridinalPosition = 0,
                                        Quoted = false
                                    },
                                    new DataItemProperty()
                                    {
                                        DataType = typeof(String),
                                        Name = "Item Type",
                                        Description = "Item Type",
                                        Path = "Item Type",
                                        PropertyType = DataItemPropertyType.Property,
                                        OridinalPosition = 1,
                                        Quoted = false
                                    },
                                    new DataItemProperty()
                                    {
                                        DataType = typeof(String),
                                        Name = "Region",
                                        Description = "Region",
                                        Path = "Region",
                                        PropertyType = DataItemPropertyType.Property,
                                        OridinalPosition = 2,
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
