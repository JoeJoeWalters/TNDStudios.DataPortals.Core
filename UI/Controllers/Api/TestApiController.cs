using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        /// Automapper set by the dependency injection to the constructor
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Default constructor with DI items
        /// </summary>
        public TestApiController(IMapper mapper)
        {
            this.mapper = mapper; // Assign the mapper from the dependency injection
        }

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
            DataItemDefinition definition = 
                new DataItemDefinition()
                {
                    ItemProperties = new List<DataItemProperty>()
                    {
                        new DataItemProperty()
                        {
                            Calculation = "",
                            DataType = typeof(Int64),
                            Description = "Primary Key",
                            Key = true,
                            Name = "Primary Key",
                            OridinalPosition = 0,
                            Path = "Primary Key",
                            Pattern = "",
                            PropertyType = DataItemPropertyType.Property,
                            Quoted = true
                        },
                        new DataItemProperty()
                        {
                            Calculation = "",
                            DataType = typeof(DateTime),
                            Description = "Date Value",
                            Key = false,
                            Name = "Date Value",
                            OridinalPosition = 1,
                            Path = "Date Value",
                            Pattern = "dd MMM yyyy",
                            PropertyType = DataItemPropertyType.Property,
                            Quoted = true
                        },
                        new DataItemProperty()
                        {
                            Calculation = "[Primary Key] * 10",
                            DataType = typeof(String),
                            Description = "Calculated Value",
                            Key = false,
                            Name = "Calculated Value",
                            OridinalPosition = 2,
                            Path = "Calculated Value",
                            Pattern = "",
                            PropertyType = DataItemPropertyType.Calculated,
                            Quoted = true
                        }
                    }
                };

            // Respond with the test data
            return new ApiResponse<DataItemDefinitionModel>()
            {
                Data = mapper.Map<DataItemDefinitionModel>(definition),
                Success = true
            };
        }
    }
}
