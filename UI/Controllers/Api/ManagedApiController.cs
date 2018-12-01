using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Json;
using TNDStudios.DataPortals.Repositories;
using TNDStudios.DataPortals.UI.Models.Api;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    /// <summary>
    /// Api Controller that managed API's are written to
    /// </summary>
    [ApiController]
    [Route("/api/package/{packageId}/managedapi")]
    public class ManagedApiController : ApiControllerBase
    {
        /// <summary>
        /// Automapper set by the dependency injection to the constructor
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Has the managed api controller been initialised?
        /// </summary>
        private static Boolean initialised = false;

        /// <summary>
        /// Holding all of the connections once provisioned
        /// </summary>
        private static DataProviderFactory providerFactory;

        /// <summary>
        /// Convert a data table to the correct format for returning to the user
        /// </summary>
        /// <returns></returns>
        private JsonResult DataTableToJsonFormat(DataTable data)
        {
            // Define the way the serialisation should be done
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = new List<JsonConverter>() { new DataRowConverter() }
            };
            
            // Send back the formatted results
            return new JsonResult(data.Rows, serializerSettings);
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ManagedApiController(IMapper mapper) : base()
        {
            this.mapper = mapper; // Assign the mapper from the dependency injection

            // Is the API system initialised?
            if (!initialised)
            {
                // Create a provider factory so that connections can be pooled
                providerFactory = new DataProviderFactory();

                // Mark the system as initialised
                initialised = true;
            }
        }

        /// <summary>
        /// Get a list (or singular) data definition model 
        /// based on a set of criteria
        /// </summary>
        /// <returns>An API response with a list of data definition models</returns>
        [HttpGet]
        [Route("definition")]
        public ApiResponse<List<CommonObjectModel>> Get([FromRoute]Guid packageId)
        {
            // Create the response object
            ApiResponse<List<CommonObjectModel>> response = new ApiResponse<List<CommonObjectModel>>();

            try
            {

                // Did we find a package?
                Package package = SessionHandler.PackageRepository.Get(packageId);
                if (package != null)
                {
                    // Was an id passed in? If not just return everything
                    response.Data = mapper.Map<List<CommonObjectModel>>(package.ApiDefinitions);
                }

                // Got to here so must be successful
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data.Clear(); // Clear the data as we don't want to send back partial data
                response.Success = false; // Failed due to hard failure
            }

            // Return the response object
            return response;
        }

        [HttpDelete]
        [Route("definition/{id}")]
        public ApiResponse<Boolean> Delete([FromRoute]Guid packageId, [FromRoute]Guid id)
        {
            // Create the response object
            ApiResponse<Boolean> response = new ApiResponse<Boolean>();

            // Did we find a package?
            Package package = SessionHandler.PackageRepository.Get(packageId);
            if (package != null)
            {
                // Get the item from the repository to make sure that it is 
                // not attached to other things
                ApiDefinition apiDefinition = package.Api(id);
                response.Success = response.Data = package.Delete<ApiDefinition>(id);
            }

            // Return the response
            return response;
        }

        [HttpGet]
        [Route("definition/{id}")]
        public ApiResponse<ApiDefinitionModel> Get([FromRoute]Guid packageId, [FromRoute]Guid id)
        {
            // Create the response object
            ApiResponse<ApiDefinitionModel> response = new ApiResponse<ApiDefinitionModel>();

            try
            {

                // Did we find a package?
                Package package = SessionHandler.PackageRepository.Get(packageId);
                if (package != null)
                {
                    // Was an id passed in? If not just return everything
                    response.Data = mapper.Map<ApiDefinitionModel>(
                        package.ApiDefinitions.Where
                            (def => (id == Guid.Empty || def.Id == id)).FirstOrDefault()
                        );

                    // Post processing to fill in the missing titles
                    // as this doesn't really fit well in Automapper due 
                    // to the source column type
                    response.Data.DataConnection =
                                mapper.Map<KeyValuePair<Guid, String>>
                                    (package.DataConnection(response.Data.DataConnection.Key));

                    response.Data.DataDefinition =
                                mapper.Map<KeyValuePair<Guid, String>>
                                    (package.DataDefinition(response.Data.DataDefinition.Key));
                }

                // Got to here so must be successful
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data = null; // Clear the data as we don't want to send back partial data
                response.Success = false; // Failed due to hard failure
            }

            // Return the response object
            return response;
        }

        [HttpPost]
        [Route("definition")]
        public ApiResponse<ApiDefinitionModel> Post([FromRoute]Guid packageId, [FromBody] ApiDefinitionModel request)
        {
            // Create the response object
            ApiResponse<ApiDefinitionModel> response = new ApiResponse<ApiDefinitionModel>();

            // Map the model to a domain object type
            ApiDefinition savedApiDefinition = mapper.Map<ApiDefinition>(request);

            // Did the mapping work ok?
            if (savedApiDefinition != null)
            {
                // Did we find a package?
                Package package = SessionHandler.PackageRepository.Get(packageId);
                if (package != null)
                {
                    // Get the repository to save the package for us
                    savedApiDefinition = package.Save<ApiDefinition>(savedApiDefinition);
                }

                // Saved ok?
                if (savedApiDefinition != null)
                {
                    // Map the api definition back to a model type and send it back to the user
                    response.Data = mapper.Map<ApiDefinitionModel>(savedApiDefinition);
                }

                // Nothing died .. Success
                response.Success = true;
            }

            // Send the response back
            return response;
        }

        [HttpGet]
        [Route("objects/{objectType}")]
        public ActionResult<Boolean> Get([FromRoute]Guid packageId, [FromRoute]String objectType)
        {
            try
            {
                // Get the package from the repository
                Package package = SessionHandler.PackageRepository.Get(packageId);
                if (package != null)
                {
                    // Get the definition for this object type
                    ApiDefinition apiDefinition = package.Api(objectType);
                    if (apiDefinition != null)
                    {
                        // Use the api definition to get the data connection and 
                        // definition from the package and then try to connect
                        IDataProvider provider = providerFactory.Get(
                            package.DataConnection(apiDefinition.DataConnection),
                            package.DataDefinition(apiDefinition.DataDefinition),
                            true);

                        // Are we connected?
                        if (provider.Connected)
                        {
                            // Return the data with the appropriate filter
                            return DataTableToJsonFormat(provider.Read(""));
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
        [Route("objects/{objectType}")]
        public ActionResult<Boolean> Post([FromRoute]String packageId, [FromRoute]String objectType)
        {
            return true;
        }

        [HttpDelete]
        [Route("objects/{objectType}")]
        public ActionResult<Boolean> Delete([FromRoute]String packageId, [FromRoute]String objectType)
        {
            return true;
        }

        [HttpPatch]
        [Route("objects/{objectType}")]
        public ActionResult<Boolean> Patch([FromRoute]String packageId, [FromRoute]String objectType)
        {
            return true;
        }

        [HttpPut]
        [Route("objects/{objectType}")]
        public ActionResult<Boolean> Put([FromRoute]String packageId, [FromRoute]String objectType)
        {
            return true;
        }
    }
}
