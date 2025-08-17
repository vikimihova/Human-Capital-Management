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

        //[HttpGet]
        //[Authorize(Roles = AdminRoleName)]
        //public async Task<IActionResult> Add()
        //{

        //}

        //[HttpPost]
        //[Authorize(Roles = AdminRoleName)]
        //public async Task<IActionResult> Add(AddRecordInputModel model)
        //{

        //}

        //[HttpGet]
        //[Authorize(Roles = AdminOrManagerRoleName)]
        //public async Task<IActionResult> Edit(string userId)
        //{

        //}

        //[HttpPost]
        //[Authorize(Roles = AdminRoleName)]
        //public async Task<IActionResult> EditByAdmin(EditRecordInputModel model)
        //{

        //}

        //[HttpPost]
        //[Authorize(Roles = ManagerRoleName)]
        //public async Task<IActionResult> EditByManager(EditRecordInputModel model)
        //{

        //}

        //[HttpPost]
        //[Authorize(Roles = AdminRoleName)]
        //public async Task<IActionResult> Delete(string userId)
        //{

        //}
    }
}
