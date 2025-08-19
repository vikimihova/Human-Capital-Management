using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.JobTitle;

using static ManagementApp.Common.ApplicationConstants;

namespace ManagementApp.Web.Controllers
{
    [Authorize(Roles = AdminRoleName)]
    public class JobTitleController : Controller
    {
        private readonly IJobTitleService jobTitleService;

        public JobTitleController(IJobTitleService jobTitleService)
        {
            this.jobTitleService = jobTitleService;
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
                return BadRequest();
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
                result = await this.jobTitleService.DeleteJobTitleAsync(id);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest();
            }

            if (!result)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Include(string id)
        {
            bool result;

            try
            {
                result = await this.jobTitleService.IncludeJobTitleAsync(id);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest();
            }

            if (!result)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
