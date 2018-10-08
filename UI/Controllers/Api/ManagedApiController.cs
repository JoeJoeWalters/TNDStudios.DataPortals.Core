using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.UI.Controllers
{
    /// <summary>
    /// Api Controller that managed API's are written to
    /// </summary>
    [ApiController]
    public class ManagedApiController : ControllerBase
    {
        private static Boolean initialised = false;
        private static Dictionary<String, ProviderSetup> connectorSetup; // What to connect to
        private static ProviderFactory providerFactory; // Holding all of the connections once provisioned

        /// <summary>
        /// Convert a data table to the correct format for returning to the user
        /// </summary>
        /// <returns></returns>
        private JsonResult DataTableToJsonFormat(DataTable data)
            => new JsonResult(data.Rows);

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ManagedApiController()
        {
            // Is the API system initialised?
            if (!initialised)
            {
                // Create a provider factory so that connections can be pooled
                providerFactory = new ProviderFactory();

                // Create a list of the available connectors (but are not yet
                // provisioned)
                connectorSetup = new Dictionary<String, ProviderSetup>()
                {
                    {
                        "flatfile" ,
                        new ProviderSetup()
                        {
                            Id = "flatfile",
                            ConnectionString = @"C:\Users\Joe\Documents\Git\TNDStudios.DataPortals.Core\FlatFileProvider.Tests\TestFiles\BigFiles\SalesRecords5000.csv",
                            Definition = new DataItemDefinition()
                            {
                                Culture = new CultureInfo("en-US"),
                                EncodingFormat = Encoding.UTF8,
                                ItemProperties = new List<DataItemProperty>()
                                {
                                    new DataItemProperty()
                                    {
                                        DataType = typeof(String),
                                        Description = "Region",
                                        Name = "Region",
                                        OridinalPosition = 0,
                                        Path = "Region",
                                        PropertyType = DataItemPropertyType.Property                                                
                                    },
                                    new DataItemProperty()
                                    {
                                        DataType = typeof(String),
                                        Description = "Country",
                                        Name = "Country",
                                        OridinalPosition = 1,
                                        Path = "Country",
                                        PropertyType = DataItemPropertyType.Property
                                    },
                                    new DataItemProperty()
                                    {
                                        DataType = typeof(String),
                                        Description = "Item Type",
                                        Name = "Item Type",
                                        OridinalPosition = 2,
                                        Path = "Item Type",
                                        PropertyType = DataItemPropertyType.Property
                                    }         
                                }
                            },
                            ProviderType = typeof(FlatFileProvider)
                        }
                    }
                };

                // Mark the system as initialised
                initialised = true;
            }
        }

        [HttpGet]
        [Route("/api/{objectType}")]
        public ActionResult<Boolean> Get(String objectType)
        {
            // Get the connector for this action
            IDataProvider provider = providerFactory.Get(connectorSetup[objectType]);
            if (provider.Connected)
            {
                return DataTableToJsonFormat(provider.Read("Country = 'Oman'"));
            }
            else
                return false;
        }

        [HttpPost]
        [Route("/api/{objectType}")]
        public ActionResult<Boolean> Post(String objectType)
        {
            return true;
        }

        [HttpDelete]
        [Route("/api/{objectType}")]
        public ActionResult<Boolean> Delete(String objectType)
        {
            return true;
        }

        [HttpPatch]
        [Route("/api/{objectType}")]
        public ActionResult<Boolean> Patch(String objectType)
        {
            return true;
        }

        [HttpPut]
        [Route("/api/{objectType}")]
        public ActionResult<Boolean> Put(String objectType)
        {
            return true;
        }
    }
}
