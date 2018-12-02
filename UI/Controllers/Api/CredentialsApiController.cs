using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNDStudios.DataPortals.Security;
using TNDStudios.DataPortals.Repositories;
using TNDStudios.DataPortals.UI.Models.Api;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    [ApiController]
    [Route("/api/package/{packageId}/credentials")]
    public class CredentialsApiController : ApiControllerBase
    {
        /// <summary>
        /// Automapper set by the dependency injection to the constructor
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CredentialsApiController(IMapper mapper) : base()
        {
            this.mapper = mapper; // Assign the mapper from the dependency injection
        }

        /// <summary>
        /// Get a list (or singular) credentials model 
        /// based on a set of criteria
        /// </summary>
        /// <returns>An API response with a list of credentials models</returns>
        [HttpGet]
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
                    response.Data = mapper.Map<List<CommonObjectModel>>(package.CredentialsStore);
                }

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

        /// <summary>
        /// Delete a credentials set
        /// </summary>
        /// <param name="id">The id of the credentials to delete</param>
        /// <returns>If the deletion was successful</returns>
        [HttpDelete]
        [Route("{id}")]
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
                Credentials credentials = package.Credentials(id);
                response.Success = response.Data = package.Delete<Credentials>(id);
            }

            // Return the response
            return response;
        }

        [HttpGet]
        [Route("{id}")]
        public ApiResponse<CredentialsModel> Get([FromRoute]Guid packageId, [FromRoute]Guid id)
        {
            // Create the response object
            ApiResponse<CredentialsModel> response = new ApiResponse<CredentialsModel>();

            try
            {
                // Did we find a package?
                Package package = SessionHandler.PackageRepository.Get(packageId);
                if (package != null)
                {
                    // Was an id passed in? If not just return everything
                    response.Data = mapper.Map<CredentialsModel>(
                        package.CredentialsStore.Where
                            (def => (id == Guid.Empty || def.Id == id)).FirstOrDefault()
                        );
                }

                response.Success = true;
            }
            catch(Exception ex)
            {
                response.Data = null; // Clear the data as we don't want to send back partial data
                response.Success = false; // Failed due to hard failure
            }

            // Return the response object
            return response;
        }
        
        [HttpPost]
        public ApiResponse<CredentialsModel> Post([FromRoute]Guid packageId, [FromBody] CredentialsModel request)
        {
            // Create the response object
            ApiResponse<CredentialsModel> response = new ApiResponse<CredentialsModel>();

            // Map the model to a domain object type
            Credentials savedCredentials = mapper.Map<Credentials>(request);

            // Did the mapping work ok?
            if (savedCredentials != null)
            {
                // Did we find a package?
                Package package = SessionHandler.PackageRepository.Get(packageId);
                if (package != null)
                {
                    // Get the repository to save the package for us
                    savedCredentials = package.Save<Credentials>(savedCredentials);
                }

                // Saved ok?
                if (savedCredentials != null)
                {
                    // Map the data definition back to a model type and send it back to the user
                    response.Data = mapper.Map<CredentialsModel>(savedCredentials);
                }

                // Nothing died .. Success
                response.Success = true;
            }

            // Send the response back
            return response;
        }
    }
}
