using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ManagementApp.Infrastructure;
using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.ApplicationUser;
using ManagementApp.Core.ViewModels.Department;
using ManagementApp.Core.ViewModels.JobTitle;
using ManagementApp.Common.CustomExceptions;

using static ManagementApp.Common.ApplicationConstants;
using static ManagementApp.Common.ErrorMessages.Logging;

namespace ManagementApp.Web.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecordApiController : ControllerBase
    {
        private readonly IRecordService recordService;
        private readonly IDepartmentService departmentService;
        private readonly IJobTitleService jobTitleService;
        private readonly ILogger<DepartmentApiController> logger;

        public RecordApiController(
            IRecordService recordService,
            IDepartmentService departmentService,
            IJobTitleService jobTitleService,
            ILogger<DepartmentApiController> logger)
        {
            this.recordService = recordService;
            this.departmentService = departmentService;
            this.jobTitleService = jobTitleService;
            this.logger = logger;
        }

        [HttpGet("index")]
        [Authorize(Roles = AdminOrManagerRoleName)]
        [ProducesResponseType(typeof(UserRecordIndexWrapper), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Index([FromBody] UserRecordIndexWrapper inputModel)
        {
            string userId = this.User.GetUserId()!;

            // generate view model
            UserRecordIndexWrapper model = new UserRecordIndexWrapper();
            ICollection<UserRecordViewModel> users;
            ICollection<SelectDepartmentViewModel> departments;
            ICollection<SelectJobTitleViewModel> jobTitles;

            try
            {
                if (this.User.IsInRole(AdminRoleName))
                {
                    users = await this.recordService.Index(userId, inputModel);
                    departments = await this.departmentService.GetDepartmentsAsync();
                    jobTitles = await this.jobTitleService.GetJobTitlesAsync();

                    model.Users = users;
                    model.Departments = departments;
                    model.JobTitles = jobTitles;
                }
                else if (this.User.IsInRole(ManagerRoleName))
                {
                    string departmentName = "";

                    users = await this.recordService.GetEmployeesByManager(userId, inputModel);
                    departmentName = await this.recordService.GetDepartmentNameByUserIdAsync(userId);
                    jobTitles = await this.jobTitleService.GetJobTitlesAsync(departmentName);

                    model.Users = users;
                    model.JobTitles = jobTitles;                    
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(RecordController)), (nameof(Index))));
                return BadRequest();
            }
            catch (Exception ex) when (ex is EntityNullException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(RecordController)), (nameof(Index))));
                return StatusCode(404);
            }

            return Ok(model);
        }

        [HttpGet("userRecord")]
        [Authorize]
        [ProducesResponseType(typeof(UserRecordViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(RecordController)), (nameof(UserRecord))));
                return BadRequest();
            }
            catch (Exception ex) when (ex is EntityNullException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(RecordController)), (nameof(UserRecord))));
                return StatusCode(404);
            }

            return Ok(model);
        }

        [HttpGet("add")]
        [Authorize(Roles = AdminRoleName)]
        [ProducesResponseType(typeof(AddRecordInputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Add()
        {
            AddRecordInputModel model = new AddRecordInputModel();

            try
            {
                model.Departments = await this.departmentService.GetDepartmentsAsync();
                model.JobTitles = await this.jobTitleService.GetJobTitlesAsync();
                model.Roles = await this.recordService.GetRolesAsync();                
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(RecordController)), (nameof(Add))));
                return BadRequest();
            }

            return Ok(model);
        }

        [HttpPost("add")]
        [Authorize(Roles = AdminRoleName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Add([FromBody] AddRecordInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result;

            try
            {
                result = await this.recordService.AddRecordAsync(model);                
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(RecordController)), (nameof(Add))));
                return BadRequest();
            }

            if (!result) return BadRequest();
            return Ok();
        }

        [HttpGet("edit/{id}")]
        [Authorize(Roles = AdminOrManagerRoleName)]
        [ProducesResponseType(typeof(EditRecordInputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit(string id)
        {
            EditRecordInputModel model;

            try
            {
                model = await this.recordService.GenerateEditRecordInputModelAsync(id);

                if (this.User.IsInRole(AdminRoleName))
                {
                    model.Departments = await this.departmentService.GetDepartmentsAsync();
                    model.JobTitles = await this.jobTitleService.GetJobTitlesAsync();
                }
                else if (this.User.IsInRole(ManagerRoleName))
                {
                    string managerUserId = this.User.GetUserId()!;
                    string departmentName = await this.recordService.GetDepartmentNameByUserIdAsync(managerUserId);
                    model.JobTitles = await this.jobTitleService.GetJobTitlesAsync(departmentName);
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(RecordController)), (nameof(Edit))));
                return BadRequest();
            }
            catch (Exception ex) when (ex is EntityNullException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(RecordController)), (nameof(Edit))));
                return StatusCode(404);
            }

            return Ok(model);
        }

        [HttpPost("edit")]
        [Authorize(Roles = AdminOrManagerRoleName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Edit([FromBody] EditRecordInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result;

            try
            {
                if (this.User.IsInRole(AdminRoleName))
                {
                    result = await this.recordService.EditRecordByAdminAsync(model);
                }
                else
                {
                    result = await this.recordService.EditRecordByManagerAsync(model);
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(RecordController)), (nameof(Edit))));
                return BadRequest();
            }

            if (!result) return BadRequest();
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = AdminRoleName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(string id)
        {
            bool result;

            try
            {
                result = await this.recordService.DeleteRecordAsync(id);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, (nameof(RecordController)), (nameof(Delete))));
                return BadRequest();
            }

            if (!result) return BadRequest();
            return Ok();
        }
    }
}
