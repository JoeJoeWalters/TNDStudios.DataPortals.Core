using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Data;

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
        /// The Id of the current collection package
        /// </summary>
        public static Guid CurrentPackageId { get; set; }

        /// <summary>
        /// Static package to hold the activity for the website
        /// </summary>
        public static List<CollectionPackage> Packages { get; set; }

        /// <summary>
        /// Get the current package object
        /// </summary>
        public static CollectionPackage CurrentPackage =>
            Packages.Where(Package => Package.Id == CurrentPackageId)
                .FirstOrDefault();

        /// <summary>
        /// Initialise the session handler
        /// </summary>
        public static void Initialise()
        {
            // Initialised?
            if (!Initialised)
            {
#warning [By default for now just create one collection package until we think about multiple orgs / environments etc.]
                CurrentPackageId = Guid.NewGuid(); // Create a default Id

                // Initialisation Id's
                Guid dataConnectionId = Guid.NewGuid();
                Guid dataDefinitionId = Guid.NewGuid();
                Guid apiId = Guid.NewGuid();

                // Set up a blank list of packages
                Packages = new List<CollectionPackage>()
                {
                    new CollectionPackage()
                    {
                        Id = CurrentPackageId,
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
                                ConnectionString = @"C:\Users\Joe\Documents\Git\TNDStudios.DataPortals.Core\FlatFileProvider.Tests\TestFiles\BigFiles\SalesRecords5000.csv",
                                Definitions = new List<Guid>() { dataDefinitionId },
                                Description = "Data Connection Description",
                                Name = "Data Connection",
                                ProviderType = DataProviderType.FlatFileProvider
                            }
                        },
                        DataDefinitions = new List<DataItemDefinition>()
                        {
                            new DataItemDefinition()
                            {
                                Id = dataDefinitionId,
                                Connections = new List<Guid>(){ dataConnectionId },
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
                        }
                    }
                };

                // Now intialised
                Initialised = true;
            }
        }
    }
}
