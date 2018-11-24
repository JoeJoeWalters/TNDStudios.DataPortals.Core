using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TNDStudios.DataPortals.Helpers;
using System.Threading.Tasks;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.UI.Models.Api;
using TNDStudios.DataPortals.Repositories;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    [ApiController]
    [Route("/api/{packageId}/data")]
    public class TransformationApiController : ApiControllerBase
    {
        /// <summary>
        /// Automapper set by the dependency injection to the constructor
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TransformationApiController(IMapper mapper) : base()
        {
            this.mapper = mapper; // Assign the mapper from the dependency injection
        }

        /// <summary>
        /// Get a list (or singular) transformation model 
        /// based on a set of criteria
        /// </summary>
        /// <returns>An API response with a list of transformation models</returns>
        [HttpGet]
        [Route("transformation")]
        public ApiResponse<List<TransformationModel>> Get([FromRoute]Guid packageId)
            => Get(packageId, Guid.Empty);
        
        [HttpDelete]
        [Route("transformation/{id}")]
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
                Transformation connection = package.Transformation(id);
                response.Success = response.Data = package.Delete<Transformation>(id);
            }

            // Return the response
            return response;
        }

        [HttpGet]
        [Route("transformation/{id}")]
        public ApiResponse<List<TransformationModel>> Get([FromRoute]Guid packageId, [FromRoute]Guid id)
        {
            // Create the response object
            ApiResponse<List<TransformationModel>> response =
                new ApiResponse<List<TransformationModel>>();

            try
            {
                // Did we find a package?
                Package package = SessionHandler.PackageRepository.Get(packageId);
                if (package != null)
                {
                    // Was an id passed in? If not just return everything
                    response.Data = mapper.Map<List<TransformationModel>>(
                        package.Transformations.Where
                            (def => (id == Guid.Empty || def.Id == id))
                        );
                }

                // Success as we got here
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

        [HttpPost]
        [Route("transformation")]
        public ApiResponse<TransformationModel> Post([FromRoute]Guid packageId, [FromBody] TransformationModel request)
        {
            // Create the response object
            ApiResponse<TransformationModel> response = new ApiResponse<TransformationModel>();

            // Map the model to a domain object type
            Transformation savedConnection = mapper.Map<Transformation>(request);

            // Did the mapping work ok?
            if (savedConnection != null)
            {

                // Did we find a package?
                Package package = SessionHandler.PackageRepository.Get(packageId);
                if (package != null)
                {
                    // Get the repository to save the package for us
                    savedConnection = package.Save<Transformation>(savedConnection);
                }

                // Saved ok?
                if (savedConnection != null)
                {
                    // Map the connection back to a model type and send it back to the user
                    response.Data = mapper.Map<TransformationModel>(savedConnection);
                }

                // Nothing died .. Success
                response.Success = true;
            }

            // Send the response back
            return response;
        }
    }
}