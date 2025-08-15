using System.ComponentModel.DataAnnotations;

using static ManagementApp.Common.EntityValidationConstants.DepartmentValidationConstants;

namespace ManagementApp.Data.DataProcessor.ImportDtos
{
    internal class DepartmentImportDto
    {
        [Required]
        [MinLength(DepartmentNameMinLength)]
        [MaxLength(DepartmentNameMaxLength)]
        public string Department { get; set; } = null!;
    }
}
