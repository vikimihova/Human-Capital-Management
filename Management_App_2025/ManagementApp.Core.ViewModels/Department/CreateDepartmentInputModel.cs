using System.ComponentModel.DataAnnotations;

using static ManagementApp.Common.EntityValidationConstants.DepartmentValidationConstants;

namespace ManagementApp.Core.ViewModels.Department
{
    public class CreateDepartmentInputModel
    {
        [Required]
        [MinLength(DepartmentNameMinLength)]
        [MaxLength(DepartmentNameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
