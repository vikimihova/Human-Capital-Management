using ManagementApp.Core.Services;
using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.Department;
using ManagementApp.Core.ViewModels.JobTitle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static ManagementApp.Common.ApplicationConstants;
using static ManagementApp.Common.ErrorMessages.Logging;

namespace ManagementApp.Web.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = AdminRoleName)]
    public class JobTitleApiController : ControllerBase
    {
        private readonly IJobTitleService jobTitleService;
        private readonly ILogger<JobTitleApiController> logger;

        public JobTitleApiController(IJobTitleService jobTitleService,
            ILogger<JobTitleApiController> logger)
        {
            this.jobTitleService = jobTitleService;
            this.logger = logger;
        }

        [HttpGet("index")]
        [ProducesResponseType(typeof(IEnumerable<JobTitleViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Index()
        {
            IEnumerable<JobTitleViewModel> model;

            try
            {
                model = await jobTitleService.Index();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, nameof(JobTitleApiController), nameof(Index)));
                return BadRequest();
            }

            return Ok(model);
        }

        [HttpGet("add")]
        [ProducesResponseType(typeof(AddJobTitleInputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Add()
        {
            AddJobTitleInputModel model = new AddJobTitleInputModel();

            return Ok(model);
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Add([FromBody] AddJobTitleInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result;

            try
            {
                result = await jobTitleService.AddJobTitleAsync(model);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, nameof(JobTitleApiController), nameof(Add)));
                return BadRequest();
            }

            if (!result) return BadRequest();
            return Ok();
        }

        [HttpGet("edit/{id}")]
        [ProducesResponseType(typeof(EditJobTitleInputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit(string id)
        {
            EditJobTitleInputModel model;

            try
            {
                model = await jobTitleService.GenerateEditJobTitleInputModelAsync(id);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, nameof(JobTitleApiController), nameof(Edit)));
                return BadRequest();
            }

            return Ok(model);
        }

        [HttpPost("edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Edit([FromBody] EditJobTitleInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result;

            try
            {
                result = await jobTitleService.EditJobTitleAsync(model);                
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, nameof(JobTitleApiController), nameof(Edit)));
                return BadRequest();
            }

            if (!result) return BadRequest();
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(string id)
        {
            bool result;

            try
            {
                result = await jobTitleService.DeleteJobTitleAsync(id);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, nameof(JobTitleApiController), nameof(Delete)));
                return BadRequest();
            }

            if (!result) return BadRequest();
            return Ok();
        }

        [HttpPost("include/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Include(string id)
        {
            bool result;

            try
            {
                result = await jobTitleService.IncludeJobTitleAsync(id);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, nameof(JobTitleApiController), nameof(Include)));
                return BadRequest();
            }

            if (!result) return BadRequest();
            return Ok();
        }
    }
}
