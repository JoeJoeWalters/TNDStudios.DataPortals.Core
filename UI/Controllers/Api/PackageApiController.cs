using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNDStudios.DataPortals.Repositories;
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
        /// Save a package to the repository
        /// </summary>
        /// <returns>The saved package mapped back as a view model</returns>
        [HttpPost]
        [Route("/api/package")]
        public ApiResponse<PackageModel> Post([FromBody] PackageModel request)
        {
            // Create the response object
            ApiResponse<PackageModel> response = new ApiResponse<PackageModel>();

            // Map the model to a domain object type
            Package savedPackage = mapper.Map<Package>(request);

            // Did the mapping work ok?
            if (savedPackage != null)
            {
                // Get the repository to save the package for us
                savedPackage = SessionHandler.PackageRepository.Save(savedPackage);
             
                // Saved ok?
                if (savedPackage != null)
                {
                    // Map the package back to a model type and send it back to the user
                    response.Data = mapper.Map<PackageModel>(savedPackage);
                }

                // Nothing died .. Success
                response.Success = true;
            }

            // Send the response back
            return response;
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
        [HttpDelete]
        [Route("/api/package/{packageId}")]
        public ApiResponse<Boolean> Delete([FromRoute]Guid packageId)
        {
            // Issue the delete request
            Boolean result = SessionHandler.PackageRepository.Delete(packageId);

            // Send back the response to the delete request
            return new ApiResponse<Boolean>()
            {
                Success = result,
                Data = result
            };
        }

        /// <summary>
        /// Get a list from the given lookup
        /// </summary>
        /// <returns>A list of a given type</returns>
        [HttpGet]
        [Route("/api/package")]
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