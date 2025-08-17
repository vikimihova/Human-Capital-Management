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
    [Authorize]
    public class RecordController : Controller
    {
        private readonly IRecordService recordService;
        private readonly IDepartmentService departmentService;
        private readonly IJobTitleService jobTitleService;
        //private readonly UserManager<ApplicationUser> userManager;

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
        [Authorize(Roles = AdminRoleName)]
        public async Task<IActionResult> IndexAll(UserRecordIndexWrapper inputModel)
        {
            // generate view model
            ICollection<UserRecordViewModel> users;
            ICollection<SelectDepartmentViewModel> departments;
            ICollection<SelectJobTitleViewModel> jobTitles;

            string userId = this.User.GetUserId()!;

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


            UserRecordIndexWrapper model = new UserRecordIndexWrapper
            {
                Users = users,
                Departments = departments,
                JobTitles = jobTitles
            };

            return View("Index", model);
        }

        [HttpGet]
        [Authorize(Roles = ManagerRoleName)]
        public async Task<IActionResult> IndexByManager(UserRecordIndexWrapper inputModel)
        {
            // generate view model
            ICollection<UserRecordViewModel> users;
            ICollection<SelectJobTitleViewModel> jobTitles;

            string userId = this.User.GetUserId()!;
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


            UserRecordIndexWrapper model = new UserRecordIndexWrapper
            {
                Users = users,
                JobTitles = jobTitles
            };

            return View("Index", model);
        }

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
    }
}
