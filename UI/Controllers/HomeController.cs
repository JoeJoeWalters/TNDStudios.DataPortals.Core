using System;
using Microsoft.AspNetCore.Mvc;

namespace TNDStudios.DataPortals.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
