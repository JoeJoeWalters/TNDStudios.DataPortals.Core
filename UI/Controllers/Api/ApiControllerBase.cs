using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApiControllerBase()
        {
            // Make sure that the session handler is initialised
            SessionHandler.Initialise(); 
        }
    }
}
