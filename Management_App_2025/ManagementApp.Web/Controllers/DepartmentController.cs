using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.ApplicationUser;
using ManagementApp.Core.ViewModels.Department;
using ManagementApp.Core.ViewModels.JobTitle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static ManagementApp.Common.ApplicationConstants;

namespace ManagementApp.Web.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }


        [HttpGet]
        [Authorize(Roles = AdminRoleName)]
        public async Task<IActionResult> Index()
        {
            // generate view model
            IEnumerable<DepartmentViewModel> model;

            try
            {
                model = await this.departmentService.Index();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest();
            }

            return View(model);
        }
    }
}
