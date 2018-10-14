using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TNDStudios.DataPortals.UI.Models;

namespace TNDStudios.DataPortals.UI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Stop swagger from freaking out about the routing with no verbs
    public class ConnectionsController : Controller
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/connections/{id}")]
        public IActionResult Connection([FromRoute]Guid id)
        {
            return View("Index");
        }
    }
}
