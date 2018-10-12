using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNDStudios.DataPortals.UI.Models.Api;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    [ApiController]
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
        [Route("/api/data/definition")]
        public ApiResponse<List<DataItemDefinitionModel>> Get()
            => Get(Guid.Empty);

        [HttpGet]
        [Route("/api/data/definition/{id}")]
        public ApiResponse<List<DataItemDefinitionModel>> Get(Guid id)
        {
            // Create the response object
            ApiResponse<List<DataItemDefinitionModel>> response =
                new ApiResponse<List<DataItemDefinitionModel>>();

            // Was an id passed in? If not just return everything
            response.Data = mapper.Map<List<DataItemDefinitionModel>>(
                SessionHandler.CurrentPackage.DataDefinitions.Where
                (def => (id == Guid.Empty || def.Id == id))
                );
            
            // Return the response object
            return response;
        }
    }
}
