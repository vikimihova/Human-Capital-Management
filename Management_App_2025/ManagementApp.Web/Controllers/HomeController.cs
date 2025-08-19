using ManagementApp.Infrastructure;
using ManagementApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ManagementApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            if (this.User?.Identity?.IsAuthenticated ?? false)
            {                
                return RedirectToAction("UserRecord", "Record");
            }

            return View();
        }


        [AllowAnonymous]
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode == 404)
            {
                return this.View("PageNotFound");
            }

            if (statusCode == 400)
            {
                return this.View("BadRequest");
            }

            if (statusCode == 500)
            {
                return this.View("InternalServerError");
            }

            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
