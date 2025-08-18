using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
