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
        [Route("/api/packages")]
        public ApiResponse<List<KeyValuePair<String, String>>> Get()
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