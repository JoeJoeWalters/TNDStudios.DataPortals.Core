using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TNDStudios.DataPortals.Helpers;
using System.Threading.Tasks;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.UI.Models.Api;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    [ApiController]
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
        [Route("/api/data/transformation")]
        public ApiResponse<List<TransformationModel>> Get()
            => Get(Guid.Empty);
        
        [HttpDelete]
        [Route("/api/data/transformation/{id}")]
        public ApiResponse<Boolean> Delete(Guid id)
        {
            // Create the response object
            ApiResponse<Boolean> response = new ApiResponse<Boolean>();

            // Get the item from the repository to make sure that it is 
            // not attached to other things
            Transformation connection = SessionHandler.CurrentPackage.Transformation(id);
            response.Success = response.Data = 
                SessionHandler.CurrentPackage.Delete<Transformation>(id);
            
            // Return the response
            return response;
        }

        [HttpGet]
        [Route("/api/data/transformation/{id}")]
        public ApiResponse<List<TransformationModel>> Get(Guid id)
        {
            // Create the response object
            ApiResponse<List<TransformationModel>> response =
                new ApiResponse<List<TransformationModel>>();

            try
            {
                // Was an id passed in? If not just return everything
                response.Data = mapper.Map<List<TransformationModel>>(
                    SessionHandler.CurrentPackage.Transformations.Where
                    (def => (id == Guid.Empty || def.Id == id))
                    );
                
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
        [Route("/api/data/transformation")]
        public ApiResponse<TransformationModel> Post([FromBody] TransformationModel request)
        {
            // Create the response object
            ApiResponse<TransformationModel> response = new ApiResponse<TransformationModel>();

            // Map the model to a domain object type
            Transformation savedConnection = mapper.Map<Transformation>(request);

            // Did the mapping work ok?
            if (savedConnection != null)
            {
                // Get the repository to save the package for us
                savedConnection = SessionHandler.CurrentPackage
                        .Save<Transformation>(savedConnection);

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