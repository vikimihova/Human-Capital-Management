using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.Department;

using static ManagementApp.Common.ApplicationConstants;

namespace ManagementApp.Web.Controllers
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
        public async Task<IActionResult> Index()
        {
            IEnumerable<DepartmentViewModel> model;

            try
            {
                model = await this.departmentService.Index();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, "Error occurred while processing department operation.");
                return BadRequest();
            }

            return Ok(model);
        }

        [HttpGet("add")]
        [ProducesResponseType(typeof(AddDepartmentInputModel), StatusCodes.Status200OK)]
        public IActionResult Add()
        {
            AddDepartmentInputModel model = new AddDepartmentInputModel();

            return Ok(model);
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] AddDepartmentInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool result = await this.departmentService.AddDepartmentAsync(model);
                if (result == false)
                {
                    return BadRequest("Failed to add department to list.");
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, "Error occurred while processing department operation.");
                return BadRequest();
            }
        }

        [HttpGet("edit/{id}")]
        [ProducesResponseType(typeof(EditDepartmentInputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Edit(string id)
        {
            EditDepartmentInputModel model;

            try
            {
                model = await this.departmentService.GenerateEditDepartmentInputModelAsync(id);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, "Error occurred while processing department operation.");
                return BadRequest();
            }

            return Ok(model);
        }

        [HttpPost("edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Edit([FromBody] EditDepartmentInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool result = await this.departmentService.EditDepartmentAsync(model);
                if (result == false)
                {
                    return BadRequest("Failed to edit department.");
                }
                else
                {                    
                    return Ok();
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, "Error occurred while processing department operation.");
                return BadRequest();
            }
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            bool result;

            try
            {
                result = await this.departmentService.DeleteDepartmentAsync(id);
                if (result == false)
                {
                    return BadRequest("Failed to delete department.");
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, "Error occurred while processing department operation.");
                return BadRequest();
            }
        }

        [HttpPost("include/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Include(string id)
        {
            bool result;

            try
            {
                result = await this.departmentService.IncludeDepartmentAsync(id);
                if (result == false)
                {
                    return BadRequest("Failed to include department.");
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                logger.LogError(ex, "Error occurred while processing department operation.");
                return BadRequest();
            }
        }
    }
}
