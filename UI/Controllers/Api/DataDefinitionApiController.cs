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
        /// Default Constructor
        /// </summary>
        public DataDefinitionApiController() : base()
        {

        }

        /// <summary>
        /// Get a list (or singular) data definition model 
        /// based on a set of criteria
        /// </summary>
        /// <returns>An API response with a list of data definition models</returns>
        [HttpGet]
        [Route("/api/data/definition")]
        public ApiResponse<List<DataItemDefinitionModel>> Get(ApiRequest<Guid> request)
        {
            // Create the response object
            ApiResponse<List<DataItemDefinitionModel>> response =
                new ApiResponse<List<DataItemDefinitionModel>>();



            // Return the response object
            return response;
        }
    }
}
