using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNDStudios.DataPortals.UI.Models;

namespace UI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Stop swagger from freaking out about the routing with no verbs
    [Route("/package")]
    public class PackagesController : Controller
    {
        [Route("index")]
        public IActionResult Transformations([FromRoute]Guid id)
        {
            // Show the view with the items needed for the page attached
            return View("Index", PackagePageVM.Create(id));
        }
    }
}