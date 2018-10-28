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
    public class ConnectionApiController : ApiControllerBase
    {
        /// <summary>
        /// Automapper set by the dependency injection to the constructor
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ConnectionApiController(IMapper mapper) : base()
        {
            this.mapper = mapper; // Assign the mapper from the dependency injection
        }

        /// <summary>
        /// Get a list of the provider types available
        /// </summary>
        /// <returns>The list of available provider types</returns>
        [HttpGet]
        [Route("/api/data/providers")]
        public ApiResponse<List<KeyValuePair<Int32, String>>> GetProviderTypes()
            => new ApiResponse<List<KeyValuePair<Int32, String>>>()
            {
                Data = typeof(DataProviderType).ToList(),
                Success = true
            };

        /// <summary>
        /// Get a list (or singular) data connection model 
        /// based on a set of criteria
        /// </summary>
        /// <returns>An API response with a list of data connection models</returns>
        [HttpGet]
        [Route("/api/data/connection")]
        public ApiResponse<List<DataConnectionModel>> Get()
            => Get(Guid.Empty);

        [HttpGet]
        [Route("/api/data/connection/{id}")]
        public ApiResponse<List<DataConnectionModel>> Get(Guid id)
        {
            // Create the response object
            ApiResponse<List<DataConnectionModel>> response =
                new ApiResponse<List<DataConnectionModel>>();

            try
            {
                // Was an id passed in? If not just return everything
                response.Data = mapper.Map<List<DataConnectionModel>>(
                    SessionHandler.CurrentPackage.DataConnections.Where
                    (def => (id == Guid.Empty || def.Id == id))
                    );

                // Post processing to fill in the missing titles
                // as this doesn't really fit well in Automapper due 
                // to the source column type
                response.Data.ForEach(item =>
                {
                    List<KeyValuePair<Guid, String>> mappedPairs =
                        new List<KeyValuePair<Guid, String>>();

                    item.Definitions.ForEach(def =>
                    {
                        mappedPairs.Add(
                            mapper.Map<KeyValuePair<Guid, String>>(SessionHandler.CurrentPackage.DataDefinition(def.Key))
                            );
                    });

                    // Assign the new list (KeyValue Pairs are readonly and the list
                    // cannot be modified in the loop to remove items so assigned here)
                    item.Definitions.Clear();
                    item.Definitions = mappedPairs;
                });

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
        [Route("/api/data/connection")]
        public ApiResponse<DataConnectionModel> Post([FromBody] ApiRequest<DataConnectionModel> request)
        {
            // Create the response object
            ApiResponse<DataConnectionModel> response = new ApiResponse<DataConnectionModel>();

            // Map the model to a domain object type
            DataConnection savedConnection = mapper.Map<DataConnection>(request.Data);

            // Did the mapping work ok?
            if (savedConnection != null)
            {
                // Get the repository to save the package for us
                savedConnection = SessionHandler.CurrentPackage
                        .Save<DataConnection>(savedConnection);

                // Saved ok?
                if (savedConnection != null)
                {
                    // Map the connection back to a model type and send it back to the user
                    response.Data = mapper.Map<DataConnectionModel>(savedConnection);
                }

                // Nothing died .. Success
                response.Success = true;
            }

            // Send the response back
            return response;
        }
    }
}