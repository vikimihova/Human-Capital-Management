using System.ComponentModel.DataAnnotations;

using static ManagementApp.Common.EntityValidationConstants.JobTitleValidationConstants;

namespace ManagementApp.Core.ViewModels.JobTitle
{
    public class EditJobTitleInputModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [MinLength(JobTitleNameMinLength)]
        [MaxLength(JobTitleNameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
