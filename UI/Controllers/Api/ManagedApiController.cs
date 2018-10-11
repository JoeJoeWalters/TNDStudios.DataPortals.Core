using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    /// <summary>
    /// Api Controller that managed API's are written to
    /// </summary>
    [ApiController]
    public class ManagedApiController : ApiControllerBase
    {
        private static Boolean initialised = false;
        private static DataProviderFactory providerFactory; // Holding all of the connections once provisioned

        /// <summary>
        /// Convert a data table to the correct format for returning to the user
        /// </summary>
        /// <returns></returns>
        private JsonResult DataTableToJsonFormat(DataTable data)
            => new JsonResult(data.Rows);

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ManagedApiController() : base()
        {
            // Is the API system initialised?
            if (!initialised)
            {
                // Create a provider factory so that connections can be pooled
                providerFactory = new DataProviderFactory();

                // Mark the system as initialised
                initialised = true;
            }
        }

        [HttpGet]
        [Route("/api/{objectType}")]
        public ActionResult<Boolean> Get(String objectType)
        {
            try
            {
                // Make sure we actually have a current package in the session
                if (SessionHandler.CurrentPackage != null)
                {
                    // Get the definition for this object type
                    ApiDefinition apiDefinition = SessionHandler.CurrentPackage.Api(objectType);
                    if (apiDefinition != null)
                    {
                        // Use the api definition to get the data connection and 
                        // definition from the package and then try to connect
                        IDataProvider provider = providerFactory.Get(
                            SessionHandler.CurrentPackage.DataConnection(apiDefinition.DataConnection),
                            SessionHandler.CurrentPackage.DataDefinition(apiDefinition.DataDefinition),
                            true);

                        // Are we connected?
                        if (provider.Connected)
                        {
                            // Return the data with the appropriate filter
                            return DataTableToJsonFormat(provider.Read("Country = 'Central America and the Caribbean'"));
                        }
                        else
                        {
                            // Could not connect to the data source, return the appropriate error code
                            return StatusCode((Int32)HttpStatusCode.InternalServerError, "Could not connect to the data source");
                        }
                    }
                    else
                    {
                        // Return a failure to find the object if we get to here
                        return StatusCode((Int32)HttpStatusCode.NotFound, $"Endpoint of type '{objectType}' was not found");
                    }
                }                
            }
            catch (Exception ex)
            {
                return StatusCode((Int32)HttpStatusCode.InternalServerError, $"Could not perform operation due to '{ex.Message}'");
            }

            // Something was very wrong that there could be no service available
            return StatusCode((Int32)HttpStatusCode.ServiceUnavailable, $"There was not package loaded to search for an endpoint of type '{objectType}'");
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
