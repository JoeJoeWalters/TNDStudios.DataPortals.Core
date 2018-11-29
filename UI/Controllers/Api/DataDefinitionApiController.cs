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
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Helpers;
using TNDStudios.DataPortals.Repositories;
using TNDStudios.DataPortals.UI.Models.Api;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    [ApiController]
    [Route("/api/package/{packageId}/data")]
    public class DataDefinitionApiController : ApiControllerBase
    {
        /// <summary>
        /// Automapper set by the dependency injection to the constructor
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataDefinitionApiController(IMapper mapper) : base()
        {
            this.mapper = mapper; // Assign the mapper from the dependency injection
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
                    response.Data = mapper.Map<List<CommonObjectModel>>(package.DataDefinitions);
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
        /// Delete a data definition
        /// </summary>
        /// <param name="id">The id of the data definition to delete</param>
        /// <returns>If the deletion was successful</returns>
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
                DataItemDefinition dataDefinition = package.DataDefinition(id);
                response.Success = response.Data = package.Delete<DataItemDefinition>(id);
            }

            // Return the response
            return response;
        }

        [HttpGet]
        [Route("definition/{id}")]
        public ApiResponse<DataItemDefinitionModel> Get([FromRoute]Guid packageId, [FromRoute]Guid id)
        {
            // Create the response object
            ApiResponse<DataItemDefinitionModel> response = new ApiResponse<DataItemDefinitionModel>();

            try
            {
                // Did we find a package?
                Package package = SessionHandler.PackageRepository.Get(packageId);
                if (package != null)
                {
                    // Was an id passed in? If not just return everything
                    response.Data = mapper.Map<DataItemDefinitionModel>(
                        package.DataDefinitions.Where
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
        [Route("definition")]
        public ApiResponse<DataItemDefinitionModel> Post([FromRoute]Guid packageId, [FromBody] DataItemDefinitionModel request)
        {
            // Create the response object
            ApiResponse<DataItemDefinitionModel> response = new ApiResponse<DataItemDefinitionModel>();

            // Map the model to a domain object type
            DataItemDefinition savedDataItemDefinition = mapper.Map<DataItemDefinition>(request);

            // Did the mapping work ok?
            if (savedDataItemDefinition != null)
            {
                // Did we find a package?
                Package package = SessionHandler.PackageRepository.Get(packageId);
                if (package != null)
                {
                    // Get the repository to save the package for us
                    savedDataItemDefinition = package.Save<DataItemDefinition>(savedDataItemDefinition);
                }

                // Saved ok?
                if (savedDataItemDefinition != null)
                {
                    // Map the data definition back to a model type and send it back to the user
                    response.Data = mapper.Map<DataItemDefinitionModel>(savedDataItemDefinition);
                }

                // Nothing died .. Success
                response.Success = true;
            }

            // Send the response back
            return response;
        }
    }
}
