using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.Department;

using static ManagementApp.Common.ApplicationConstants;
using static ManagementApp.Common.ErrorMessages.Logging;

namespace ManagementApp.Web.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = AdminRoleName)]
    public class DepartmentApiController : ControllerBase
    {
        private readonly IDepartmentService departmentService;
        private readonly ILogger<DepartmentApiController> logger;

        public DepartmentApiController(IDepartmentService departmentService,
            ILogger<DepartmentApiController> logger)
        {
            this.departmentService = departmentService;
            this.logger = logger;
        }

        [HttpGet("index")]
        [ProducesResponseType(typeof(IEnumerable<DepartmentViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Index()
        {
            IEnumerable<DepartmentViewModel> model;

            try
            {
                model = await departmentService.Index();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, nameof(DepartmentApiController), nameof(Index)));
                return BadRequest();
            }

            return Ok(model);
        }

        [HttpGet("add")]
        [ProducesResponseType(typeof(AddDepartmentInputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Add()
        {
            AddDepartmentInputModel model = new AddDepartmentInputModel();

            return Ok(model);
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Add([FromBody] AddDepartmentInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result;

            try
            {
                result = await departmentService.AddDepartmentAsync(model);                
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, nameof(DepartmentApiController), nameof(Add)));
                return BadRequest();
            }

            if (!result) return BadRequest();
            return Ok();
        }

        [HttpGet("edit/{id}")]
        [ProducesResponseType(typeof(EditDepartmentInputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit(string id)
        {
            EditDepartmentInputModel model;

            try
            {
                model = await departmentService.GenerateEditDepartmentInputModelAsync(id);                
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, nameof(DepartmentApiController), nameof(Edit)));
                return BadRequest();
            }

            return Ok(model);
        }

        [HttpPost("edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Edit([FromBody] EditDepartmentInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result;

            try
            {
                result = await departmentService.EditDepartmentAsync(model);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, nameof(DepartmentApiController), nameof(Edit)));
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
                result = await departmentService.DeleteDepartmentAsync(id);                
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, nameof(DepartmentApiController), nameof(Delete)));
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
                result = await departmentService.IncludeDepartmentAsync(id);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, string.Format(ErrorLogMessage, ex.Message, nameof(DepartmentApiController), nameof(Include)));
                return BadRequest();
            }


            if (!result) return BadRequest();
            return Ok();
        }
    }
}
