using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ManagementApp.Web.Models;

namespace ManagementApp.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (this.User?.Identity?.IsAuthenticated ?? false)
            {                
                return RedirectToAction("UserRecord", "Record");
            }

            return View();
        }
        
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
