using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.JobTitle;
using ManagementApp.Common.CustomExceptions;

using static ManagementApp.Common.ApplicationConstants;
using static ManagementApp.Common.ErrorMessages.Logging;

namespace ManagementApp.Web.Controllers
{
    [Authorize(Roles = AdminRoleName)]
    public class JobTitleController : Controller
    {
        private readonly IJobTitleService jobTitleService;
        private readonly ILogger<JobTitleController> logger;

        public JobTitleController(IJobTitleService jobTitleService,
            ILogger<JobTitleController> logger)
        {
            this.jobTitleService = jobTitleService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // generate view model
            IEnumerable<JobTitleViewModel> model;

            try
            {
                model = await this.jobTitleService.Index();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(JobTitleController)), (nameof(Index))));
                return BadRequest();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            AddJobTitleInputModel model = new AddJobTitleInputModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddJobTitleInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                bool result = await this.jobTitleService.AddJobTitleAsync(model);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(JobTitleController)), (nameof(Add))));
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            EditJobTitleInputModel model;

            try
            {
                model = await this.jobTitleService.GenerateEditJobTitleInputModelAsync(id);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(JobTitleController)), (nameof(Edit))));
                return BadRequest();
            }
            catch (Exception ex) when (ex is EntityNullException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(JobTitleController)), (nameof(Edit))));
                return StatusCode(404);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditJobTitleInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                bool result = await this.jobTitleService.EditJobTitleAsync(model);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(JobTitleController)), (nameof(Edit))));
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        // requires a form input
        // feature currently handled by the ApiController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            bool result;

            try
            {
                result = await this.jobTitleService.DeleteJobTitleAsync(id);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(JobTitleController)), (nameof(Delete))));
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        // requires a form input
        // feature currently handled by the ApiController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Include(string id)
        {
            bool result;

            try
            {
                result = await this.jobTitleService.IncludeJobTitleAsync(id);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(JobTitleController)), (nameof(Include))));
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
