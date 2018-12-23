using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.PropertyBag;
using TNDStudios.DataPortals.UI.Models.Api;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    /// <summary>
    /// Api Controller that managed API's are written to
    /// </summary>
    [ApiController]
    [Route("/api/system")]
    public class SystemApiController : ApiControllerBase
    {
        /// <summary>
        /// Automapper set by the dependency injection to the constructor
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SystemApiController(IMapper mapper) : base()
        {
            this.mapper = mapper; // Assign the mapper from the dependency injection
        }

        /// <summary>
        /// Get a list from the given lookup
        /// </summary>
        /// <returns>A list of a given type</returns>
        [HttpGet]
        [Route("lookup/{id}")]
        public ApiResponse<List<KeyValuePair<String, String>>> GetLookup([FromRoute] LookupFactoryType id)
            => new ApiResponse<List<KeyValuePair<String, String>>>()
            {
                Data = (new LookupFactory()).Get(id),
                Success = true
            };

        /// <summary>
        /// Get a list from the given lookup
        /// </summary>
        /// <returns>A list of a given type</returns>
        [HttpGet]
        [Route("propertybag/{objectType}/{value}")]
        public ApiResponse<List<PropertyBagItemTypeModel>> GetPropertyBag([FromRoute] ObjectTypes objectType, [FromRoute] Int32 value)
            => new ApiResponse<List<PropertyBagItemTypeModel>>()
            {
                Data = mapper.Map<List<PropertyBagItemTypeModel>>((new PropertyBagFactory()).Get(objectType, value)),
                Success = true
            };

        /// <summary>
        /// Get information about the service data provider given
        /// </summary>
        [HttpGet]
        [Route("provider/{providerId}/summary")]
        public ApiResponse<DataProviderModel> GetDataProviderSummary(DataProviderType providerId)
            => new ApiResponse<DataProviderModel>(
                mapper.Map<DataProviderModel>(
                    (new DataProviderFactory()).Get(providerId)
                    )
                );
    }
}