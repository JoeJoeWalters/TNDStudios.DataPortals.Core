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
    public class DataDefinitionsController : Controller
    {
        [Route("/datadefinitions/{id}")]
        public IActionResult DataDefinition([FromRoute]Guid id)
        {
            return View("Index");
        }
    }
}
