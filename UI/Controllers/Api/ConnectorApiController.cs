using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.UI.Models.Api;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    [ApiController]
    public class ConnectorApiController : ApiControllerBase
    {
        /// <summary>
        /// Automapper set by the dependency injection to the constructor
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ConnectorApiController(IMapper mapper) : base()
        {
            this.mapper = mapper; // Assign the mapper from the dependency injection
        }

        /// <summary>
        /// Get a list (or singular) data connection model 
        /// based on a set of criteria
        /// </summary>
        /// <returns>An API response with a list of data connection models</returns>
        [HttpGet]
        [Route("/api/data/connector")]
        public ApiResponse<List<DataConnectionModel>> Get()
            => Get(Guid.Empty);

        [HttpGet]
        [Route("/api/data/connector/{id}")]
        public ApiResponse<List<DataConnectionModel>> Get(Guid id)
        {
            // Create the response object
            ApiResponse<List<DataConnectionModel>> response =
                new ApiResponse<List<DataConnectionModel>>();

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
                    // Lookup the objects from the package
                    DataItemDefinition definition = SessionHandler.CurrentPackage.DataDefinition(def.Key);

                    // Assign the correct values to the model
                    mappedPairs.Add(new KeyValuePair<Guid, string>(definition.Id, definition.Name));
                });

                // Assign the new list (KeyValue Pairs are readonly and the list
                // cannot be modified in the loop to remove items so assigned here)
                item.Definitions.Clear();
                item.Definitions = mappedPairs;
            });

            // Return the response object
            return response;

        }
    }
}
