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
using TNDStudios.DataPortals.Repositories;
using TNDStudios.DataPortals.Security;
using TNDStudios.DataPortals.UI.Controllers.Api.Helpers;
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
        /// Reference to the helpers object
        /// </summary>
        private static ManagedApiHelpers helpers;

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

                // Set up an instance of the helper class across all sessions
                helpers = new ManagedApiHelpers();

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

        /// <summary>
        /// Handle the GET verb of the managed Api by checking permissions and then
        /// translating the Api Definition in to a readable form to the caller
        /// </summary>
        /// <param name="packageId">The package that is being referenced containing the Api Definition</param>
        /// <param name="objectType">The "name" of the Api endpoint being referenced</param>
        /// <returns></returns>
        [HttpGet]
        [Route("objects/{objectType}")]
        public ActionResult<Boolean> Get([FromRoute]Guid packageId, [FromRoute]String objectType)
        {
            // Do the authentication process with the given request and helpers 
            // to determine the result
            ApiAuthenticationResult authResult =
                webAuthHelper.AuthenticateApiRequest(
                    packageId,
                    objectType,
                    SessionHandler.PackageRepository,
                    Request
                    );

            // Everything work ok? Then continue to the important bits
            if (authResult.StatusCode == HttpStatusCode.OK)
            {
                // Use the api definition to get the data connection and 
                // definition from the package and then try to connect
                IDataProvider provider = providerFactory.Get(
                    authResult.Package,
                    authResult.Package.DataConnection(authResult.ApiDefinition.DataConnection),
                    authResult.Package.DataDefinition(authResult.ApiDefinition.DataDefinition),
                    true);

                // Are we connected?
                if (provider.Connected)
                {
                    // Return the data with the appropriate filter
                    DataTable results = provider.Read(authResult.Permissions.Filter);

                    // Manage any aliases for the results table
                    helpers.HandleAliases(results, authResult.ApiDefinition.Aliases);

                    // Format the data table as Json
                    return helpers.DataTableToJsonFormat(results);
                }
                else
                {
                    // Could not connect to the data source, return the appropriate error code
                    return StatusCode((Int32)HttpStatusCode.InternalServerError, "Could not connect to the data source");
                }
            }
            else
                return StatusCode((Int32)authResult.StatusCode, authResult.StatusDescription);            
        }

        /// <summary>
        /// Handle the POST verb of the managed Api by checking permissions and then
        /// translating the request to save data in to a process to save that data to the data source
        /// </summary>
        /// <param name="packageId">The package that is being referenced containing the Api Definition</param>
        /// <param name="objectType">The "name" of the Api endpoint being referenced</param>
        /// <returns></returns>
        [HttpPost]
        [Route("objects/{objectType}")]
        public ActionResult<Boolean> Post([FromRoute]Guid packageId, [FromRoute]String objectType)
        {
            // Do the authentication process with the given request and helpers 
            // to determine the result
            ApiAuthenticationResult authResult =
                webAuthHelper.AuthenticateApiRequest(
                    packageId,
                    objectType,
                    SessionHandler.PackageRepository,
                    Request
                    );

            // Everything work ok? Then continue to the important bits
            if (authResult.StatusCode == HttpStatusCode.OK)
            {
                return StatusCode((Int32)HttpStatusCode.OK, "");
            }
            else
                return StatusCode((Int32)authResult.StatusCode, authResult.StatusDescription);
        }

        /// <summary>
        /// Handle the DELETE verb of the managed Api by checking permissions and then
        /// translating the request to delete data in to a process to delete data in the data source
        /// </summary>
        /// <param name="packageId">The package that is being referenced containing the Api Definition</param>
        /// <param name="objectType">The "name" of the Api endpoint being referenced</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("objects/{objectType}")]
        public ActionResult<Boolean> Delete([FromRoute]Guid packageId, [FromRoute]String objectType)
        {
            // Do the authentication process with the given request and helpers 
            // to determine the result
            ApiAuthenticationResult authResult =
                webAuthHelper.AuthenticateApiRequest(
                    packageId,
                    objectType,
                    SessionHandler.PackageRepository,
                    Request
                    );

            // Everything work ok? Then continue to the important bits
            if (authResult.StatusCode == HttpStatusCode.OK)
            {
                return StatusCode((Int32)HttpStatusCode.OK, "");
            }
            else
                return StatusCode((Int32)authResult.StatusCode, authResult.StatusDescription);
        }

        /// <summary>
        /// Handle the PATCH verb of the managed Api by checking permissions and then
        /// translating the request to update data in to a process to update that data to the data source
        /// </summary>
        /// <param name="packageId">The package that is being referenced containing the Api Definition</param>
        /// <param name="objectType">The "name" of the Api endpoint being referenced</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("objects/{objectType}")]
        public ActionResult<Boolean> Patch([FromRoute]Guid packageId, [FromRoute]String objectType)
        {
            // Do the authentication process with the given request and helpers 
            // to determine the result
            ApiAuthenticationResult authResult =
                webAuthHelper.AuthenticateApiRequest(
                    packageId,
                    objectType,
                    SessionHandler.PackageRepository,
                    Request
                    );

            // Everything work ok? Then continue to the important bits
            if (authResult.StatusCode == HttpStatusCode.OK)
            {
                return StatusCode((Int32)HttpStatusCode.OK, "");
            }
            else
                return StatusCode((Int32)authResult.StatusCode, authResult.StatusDescription);
        }
    }
}
