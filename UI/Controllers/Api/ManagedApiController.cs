using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Json;
using TNDStudios.DataPortals.Repositories;
using TNDStudios.DataPortals.Security;
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
        /// Helper classes that are set up and used often across multiple sessions
        /// </summary>
        private static WebAuthHelper webAuthHelper;

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
                // Create an instance of the web authorisation helper (static as it will be used quite often
                // and this controller is session based)
                webAuthHelper = new WebAuthHelper();

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
            catch
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

                    // Loop through the credential links
                    response.Data.CredentialsLinks.ForEach(credentialsLink =>
                    {
                        // Map the credentials object to get the title etc.
                        credentialsLink.Credentials = mapper.Map<KeyValuePair<Guid, String>>
                            (package.Credentials(credentialsLink.Credentials.Key));
                    });
                }

                // Got to here so must be successful
                response.Success = true;
            }
            catch
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

        /// <summary>
        /// Preview an Api end point using a set of credentials without needing to know the actual credentials
        /// </summary>
        /// <param name="objectType">The key of the api</param>
        /// <returns></returns>
        [HttpGet]
        [Route("objects/{objectType}/preview/{credentialsId}")]
        public ActionResult<Boolean> Preview(
            [FromRoute]Guid packageId,
            [FromRoute]String objectType,
            [FromRoute]Guid credentialsId)
        {
            // Get the package from the repository
            Package package = SessionHandler.PackageRepository.Get(packageId);
            if (package != null)
            {
                // Get the credentials for this api Definition
                Credentials credentials = package.Credentials(credentialsId);
                if (credentials != null)
                {
                    // Get the username and password from the credentials
                    String username = credentials.GetValue("username");
                    String password = credentials.GetValue("password");

                    // Work out the endpoint to call (it's this controller with a different endpoint)
                    String protocol = (HttpContext.Request.IsHttps ? "https" : "http");
                    String path = $"{protocol}://{HttpContext.Request.Host}/api/package/{packageId.ToString()}/managedapi/objects/{objectType}";

                    // Create a web request to the actual endpoint location
                    WebRequest webRequest = WebRequest.Create(path);

                    // Set the content type to be json as if it was real
                    webRequest.ContentType = "application/json";

                    // Set the authorization values based on the credentials to preview as
                    var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                    webRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(byteArray));

                    // Define some response text
                    String responseText = String.Empty;
                    try
                    {
                        // Call and get the response from the api
                        WebResponse webResponse = webRequest.GetResponse();

                        // Get the content from the api as it comes in 
                        using (StreamReader reader =
                            new StreamReader(webResponse.GetResponseStream(), ASCIIEncoding.ASCII))
                        {
                            responseText = reader.ReadToEnd(); // Read all of the text
                        }

                        // Close the web response
                        webResponse.Close();
                    }
                    catch (Exception ex)
                    {
                        if (ex.GetType() == typeof(WebException))
                        {
                            WebException webException = (WebException)ex;
                            if (webException != null)
                            {
                                HttpStatusCode statusCode = ((HttpWebResponse)webException.Response).StatusCode;
                                return StatusCode((Int32)statusCode, webException.Message);
                            }
                            else
                                return StatusCode((Int32)HttpStatusCode.ServiceUnavailable, "Failure in preview service. Please contact your administrator.");
                        }
                        else
                            return StatusCode((Int32)HttpStatusCode.ServiceUnavailable, "Failure in preview service. Please contact your administrator.");
                    }

                    // Return the data to the caller
                    return Content(responseText);
                }
                else
                    return StatusCode((Int32)HttpStatusCode.Unauthorized, $"There were no valid credentials assigned for endpoint of type '{objectType}'");
            }

            // Something was very wrong that there could be no service available
            return StatusCode((Int32)HttpStatusCode.ServiceUnavailable, $"There was not package loaded to search for an endpoint of type '{objectType}'");
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
                        // Authenticate this request against the Api Definition
                        Permissions permissions = webAuthHelper.AuthenticateRequest(Request, package, apiDefinition);
                        if (permissions != null && permissions.CanRead)
                        {
                            // Use the api definition to get the data connection and 
                            // definition from the package and then try to connect
                            IDataProvider provider = providerFactory.Get(
                                package,
                                package.DataConnection(apiDefinition.DataConnection),
                                package.DataDefinition(apiDefinition.DataDefinition),
                                true);

                            // Are we connected?
                            if (provider.Connected)
                            {
                                // Return the data with the appropriate filter
                                DataTable results = provider.Read(permissions.Filter);

                                // Loop the alias's for this Api and inject them
                                apiDefinition.Aliases.ForEach(pair => 
                                {
                                    // Do we have a column with the correct name
                                    if (results.Columns.Contains(pair.Key))
                                    {
                                        // Get the column
                                        DataColumn column = results.Columns[pair.Key];
                                        if (column != null)
                                            column.ExtendedProperties["Alias"] = pair.Value;
                                    }
                                });

                                // Format the data table as Json
                                return DataTableToJsonFormat(results);
                            }
                            else
                            {
                                // Could not connect to the data source, return the appropriate error code
                                return StatusCode((Int32)HttpStatusCode.InternalServerError, "Could not connect to the data source");
                            }
                        }
                        else
                            // Return a failure to authenticate
                            return StatusCode((Int32)HttpStatusCode.Unauthorized, $"Endpoint of type '{objectType}' was not authorized for these credentials");
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
