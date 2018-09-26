using Microsoft.AspNetCore.Mvc;
using System;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.UI.Controllers.RequestResponse;
using TNDStudios.DataPortals.UI.Models.Api;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    public class TestData
    {
        public String Value { get; set; }
    }

    [ApiController]
    public class TestApiController : ControllerBase
    {
        /// <summary>
        /// Test harness to get a test definition back so we can test
        /// the MVVM pattern
        /// </summary>
        /// <param name="request">A void request</param>
        /// <returns>A set of test data</returns>
        [HttpGet]
        [Route("/api/test/definition")]
        public ApiResponse<DataItemDefinitionModel> TestDefinition([FromQuery] ApiRequest<Object> request)
        {
            // Send some test data back
            return new ApiResponse<DataItemDefinitionModel>(
                new DataItemDefinitionModel()
                {
                });
        }
    }
}
