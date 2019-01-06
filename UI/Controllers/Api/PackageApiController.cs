using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNDStudios.DataPortals.UI;
using TNDStudios.DataPortals.UI.Controllers;
using TNDStudios.DataPortals.UI.Controllers.Api;
using TNDStudios.DataPortals.UI.Models.Api;

namespace UI.Controllers.Api
{
    /// <summary>
    /// Manages the packages that contain the definitions 
    /// </summary>
    [ApiController]
    public class PackageApiController : ApiControllerBase
    {
        /// <summary>
        /// Automapper set by the dependency injection to the constructor
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PackageApiController(IMapper mapper) : base()
        {
            this.mapper = mapper; // Assign the mapper from the dependency injection
        }

        /// <summary>
        /// Get a list from the given lookup
        /// </summary>
        /// <returns>A list of a given type</returns>
        [HttpGet]
        [Route("/api/package/{packageId}")]
        public ApiResponse<PackageModel> Get([FromRoute]Guid packageId)
            => new ApiResponse<PackageModel>()
            {
                Data = mapper.Map<PackageModel>(
                    SessionHandler.PackageRepository.Get(packageId)
                    ),
                Success = true
            };

        /// <summary>
        /// Get a list from the given lookup
        /// </summary>
        /// <returns>A list of a given type</returns>
        [HttpGet]
        [Route("/api/package/list")]
        public ApiResponse<List<KeyValuePair<String, String>>> List()
            => new ApiResponse<List<KeyValuePair<String, String>>>()
            {
                Data = SessionHandler.PackageRepository
                    .Get()
                    .Select(package => new KeyValuePair<string, string>(package.Id.ToString(), package.Name))
                    .ToList(),
                Success = true
            };
    }
}