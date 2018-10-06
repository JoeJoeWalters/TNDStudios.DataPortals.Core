using Microsoft.AspNetCore.Mvc;
using System;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    /// <summary>
    /// Api Controller that managed API's are written to
    /// </summary>
    [ApiController]
    public class ManagedApiController : ControllerBase
    {
        [HttpGet]
        [Route("/api/{objectType}")]
        public ActionResult<Boolean> Get(String objectType)
        {
            return true;
        }

        [HttpPost]
        [Route("/api/{objectType}")]
        public ActionResult<Boolean> Post(String objectType)
        {
            return true;
        }

        [HttpDelete]
        [Route("/api/{objectType}")]
        public ActionResult<Boolean> Delete(String objectType)
        {
            return true;
        }

        [HttpPatch]
        [Route("/api/{objectType}")]
        public ActionResult<Boolean> Patch(String objectType)
        {
            return true;
        }

        [HttpPut]
        [Route("/api/{objectType}")]
        public ActionResult<Boolean> Put(String objectType)
        {
            return true;
        }
    }
}
