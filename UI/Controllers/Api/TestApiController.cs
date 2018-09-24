using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Controllers.Api
{
    public class TestData
    {
        public String value { get; set; }
    }

    [ApiController]
    public class TestApiController : ControllerBase
    {
        [HttpGet]
        [Route("/Api/TestData")]
        public TestData TestData()
        {
            return new TestData() { value = "Test Data" };
        }
    }
}
