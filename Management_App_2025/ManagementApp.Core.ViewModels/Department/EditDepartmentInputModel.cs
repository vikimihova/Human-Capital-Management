using System.ComponentModel.DataAnnotations;

using static ManagementApp.Common.EntityValidationConstants.DepartmentValidationConstants;

namespace ManagementApp.Core.ViewModels.Department
{
    public class EditDepartmentInputModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [MinLength(DepartmentNameMinLength)]
        [MaxLength(DepartmentNameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
