using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagementApp.Web.Controllers
{
    [Authorize]
    public class JobTitleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
