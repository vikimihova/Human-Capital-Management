using System.ComponentModel.DataAnnotations;

using static ManagementApp.Common.EntityValidationConstants.JobTitleValidationConstants;

namespace ManagementApp.Core.ViewModels.JobTitle
{
    public class EditJobTitleInputModel
    {
        [Required]
        [MinLength(JobTitleNameMinLength)]
        [MaxLength(JobTitleNameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
