using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ManagementApp.Common.CustomExceptions;
using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.Department;

using static ManagementApp.Common.ApplicationConstants;
using static ManagementApp.Common.ErrorMessages.Logging;

namespace ManagementApp.Web.Controllers
{
    [Authorize(Roles = AdminRoleName)]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService departmentService;
        private readonly ILogger<DepartmentController> logger;

        public DepartmentController(IDepartmentService departmentService,
            ILogger<DepartmentController> logger)
        {
            this.departmentService = departmentService;
            this.logger = logger;
        }

        [HttpGet]
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
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(DepartmentController)), (nameof(Index))));
                return BadRequest();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            AddDepartmentInputModel model = new AddDepartmentInputModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddDepartmentInputModel model)
        {
            if (!ModelState.IsValid)
            {                
                return View(model);
            }

            try
            {
                bool result = await this.departmentService.AddDepartmentAsync(model);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(DepartmentController)), (nameof(Add))));
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            EditDepartmentInputModel model;

            try
            {
                model = await this.departmentService.GenerateEditDepartmentInputModelAsync(id);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(DepartmentController)), (nameof(Edit))));
                return BadRequest();
            }
            catch (Exception ex) when (ex is EntityNullException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(DepartmentController)), (nameof(Edit))));
                return StatusCode(404);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditDepartmentInputModel model)
        {
            if (!ModelState.IsValid)
            {               
                return View(model);
            }

            try
            {
                bool result = await this.departmentService.EditDepartmentAsync(model);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(DepartmentController)), (nameof(Edit))));
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            bool result;

            try
            {
                result = await this.departmentService.DeleteDepartmentAsync(id);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(DepartmentController)), (nameof(Delete))));
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Include(string id)
        {
            bool result;

            try
            {
                result = await this.departmentService.IncludeDepartmentAsync(id);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(DepartmentController)), (nameof(Include))));
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
