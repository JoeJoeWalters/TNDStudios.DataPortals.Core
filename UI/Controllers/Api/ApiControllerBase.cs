using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    // https://www.blinkingcaret.com/2017/09/06/secure-web-api-in-asp-net-core/
    // http://jasonwatmore.com/post/2018/08/14/aspnet-core-21-jwt-authentication-tutorial-with-example-api

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
