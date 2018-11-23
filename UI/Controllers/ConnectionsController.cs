using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TNDStudios.DataPortals.UI.Models;
using TNDStudios.DataPortals.UI.Models.Api;

namespace TNDStudios.DataPortals.UI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Stop swagger from freaking out about the routing with no verbs
    public class ConnectionsController : Controller
    {
        [Route("/packages/{packageId}/connections/{id}")]
        public IActionResult Connection([FromRoute]Guid packageId, [FromRoute]Guid id)
        {
            // Show the view with the items needed for the page attached
            return View("Index", PackagePageVM.Create(packageId));
        }

    }
}
