using System.ComponentModel.DataAnnotations;

using static ManagementApp.Common.EntityValidationConstants.JobTitleValidationConstants;

namespace ManagementApp.Data.DataProcessor.ImportDtos
{
    internal class JobTitleImportDto
    {
        [Required]
        [MinLength(JobTitleNameMinLength)]
        [MaxLength(JobTitleNameMaxLength)]
        public string JobTitle { get; set; } = null!;
    }
}
