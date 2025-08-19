using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.ApplicationUser;
using ManagementApp.Core.ViewModels.Department;
using ManagementApp.Core.ViewModels.JobTitle;
using ManagementApp.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static ManagementApp.Common.ApplicationConstants;
using static NuGet.Packaging.PackagingConstants;

namespace ManagementApp.Web.Controllers
{    
    public class RecordController : Controller
    {
        private readonly IRecordService recordService;
        private readonly IDepartmentService departmentService;
        private readonly IJobTitleService jobTitleService;

        public RecordController(
            IRecordService recordService,
            IDepartmentService departmentService,
            IJobTitleService jobTitleService)
        {
            this.recordService = recordService;
            this.departmentService = departmentService;
            this.jobTitleService = jobTitleService;
        }        

        [HttpGet]
        [Authorize(Roles = AdminOrManagerRoleName)]
        public async Task<IActionResult> Index(UserRecordIndexWrapper inputModel)
        {
            string userId = this.User.GetUserId()!;

            // generate view model
            UserRecordIndexWrapper model = new UserRecordIndexWrapper();
            ICollection<UserRecordViewModel> users;
            ICollection<SelectDepartmentViewModel> departments;
            ICollection<SelectJobTitleViewModel> jobTitles;

            if (this.User.IsInRole(AdminRoleName))
            {
                try
                {
                    users = await this.recordService.Index(userId, inputModel);
                    departments = await this.departmentService.GetDepartmentsAsync();
                    jobTitles = await this.jobTitleService.GetJobTitlesAsync();
                }
                catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
                {
                    return BadRequest();
                }

                model.Users = users;
                model.Departments = departments;
                model.JobTitles = jobTitles;
            }
            else if (this.User.IsInRole(ManagerRoleName))
            {
                string departmentName = "";

                try
                {
                    users = await this.recordService.GetEmployeesByManager(userId, inputModel);
                    departmentName = await this.recordService.GetDepartmentNameByUserIdAsync(userId);
                    jobTitles = await this.jobTitleService.GetJobTitlesAsync(departmentName);
                }
                catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
                {
                    return BadRequest();
                }

                model.Users = users;
                model.JobTitles= jobTitles;
            }            

            return View("Index", model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserRecord()
        {
            // get userId
            string userId = this.User.GetUserId()!;

            // generate view model
            UserRecordViewModel model;

            try
            {
                model = await this.recordService.GetUserByIdAsync(userId);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest();
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = AdminRoleName)]
        public async Task<IActionResult> Add()
        {
            AddRecordInputModel model = new AddRecordInputModel();

            model.Departments = await this.departmentService.GetDepartmentsAsync();
            model.JobTitles = await this.jobTitleService.GetJobTitlesAsync();
            model.Roles = await this.recordService.GetRolesAsync();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdminRoleName)]
        public async Task<IActionResult> Add(AddRecordInputModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Departments = await this.departmentService.GetDepartmentsAsync();
                model.JobTitles = await this.jobTitleService.GetJobTitlesAsync();
                model.Roles = await this.recordService.GetRolesAsync();

                return View(model);
            }

            try
            {
                bool result = await this.recordService.AddRecordAsync(model);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = AdminOrManagerRoleName)]
        public async Task<IActionResult> Edit(string id)
        {
            EditRecordInputModel model;

            try
            {
                model = await this.recordService.GenerateEditRecordInputModelAsync(id);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest();
            }

            if (this.User.IsInRole(AdminRoleName))
            {
                model.Departments = await this.departmentService.GetDepartmentsAsync();
                model.JobTitles = await this.jobTitleService.GetJobTitlesAsync();

                return View("EditByAdmin", model);
            }
            else if (this.User.IsInRole(ManagerRoleName))
            {
                string managerUserId = this.User.GetUserId()!;
                string departmentName = await this.recordService.GetDepartmentNameByUserIdAsync(managerUserId);
                model.JobTitles = await this.jobTitleService.GetJobTitlesAsync(departmentName);
            }

            return View("EditByManager", model);
        }

        [HttpPost]
        [Authorize(Roles = AdminOrManagerRoleName)]
        public async Task<IActionResult> Edit(EditRecordInputModel model)
        {
            if (!ModelState.IsValid)
            {
                if (this.User.IsInRole(AdminRoleName))
                {
                    model.Departments = await this.departmentService.GetDepartmentsAsync();
                    model.JobTitles = await this.jobTitleService.GetJobTitlesAsync();

                    return View("EditByAdmin", model);
                }
                else if (this.User.IsInRole(ManagerRoleName))
                {
                    string managerUserId = this.User.GetUserId()!;
                    string departmentName = await this.recordService.GetDepartmentNameByUserIdAsync(managerUserId);
                    model.JobTitles = await this.jobTitleService.GetJobTitlesAsync(departmentName);

                    return View("EditByManager", model);
                }
            }

            try
            {
                if (this.User.IsInRole(AdminRoleName))
                {
                    bool result = await this.recordService.EditRecordByAdminAsync(model);
                }
                else if (this.User.IsInRole(ManagerRoleName))
                {
                    bool result = await this.recordService.EditRecordByManagerAsync(model);
                }                
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = AdminRoleName)]
        public async Task<IActionResult> Delete(string id)
        {
            bool result;

            try
            {
                result = await this.recordService.DeleteRecordAsync(id);
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
