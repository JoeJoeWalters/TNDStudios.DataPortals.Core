using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    /// <summary>
    /// Api Controller that managed API's are written to
    /// </summary>
    [ApiController]
    public class SystemApiController : ApiControllerBase
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SystemApiController() : base()
        {
        }

        /// <summary>
        /// Get a list from the given lookup
        /// </summary>
        /// <returns>A list of a given type</returns>
        [HttpGet]
        [Route("/api/system/lookup/{id}")]
        public ApiResponse<List<KeyValuePair<String, String>>> Get(LookupFactoryType id)
            => new ApiResponse<List<KeyValuePair<String, String>>>()
            {
                Data = (new LookupFactory()).Get(id),
                Success = true
            };
    }
}