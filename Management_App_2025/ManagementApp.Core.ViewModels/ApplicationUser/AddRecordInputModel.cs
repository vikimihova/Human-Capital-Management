using System.ComponentModel.DataAnnotations;

using static ManagementApp.Common.EntityValidationConstants.UserValidationConstants;

namespace ManagementApp.Core.ViewModels.ApplicationUser
{
    public class AddRecordInputModel
    {
        [Required]
        [MinLength(UserFirstNameMinLength)]
        [MaxLength(UserFirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(UserLastNameMinLength)]
        [MaxLength(UserLastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        public string JobTitleId { get; set; } = null!;

        [Required]
        public string JobTitle { get; set; } = null!;

        [Required]
        [Range(SalaryMinAmount, SalaryMaxAmount)]
        public int Salary { get; set; }

        [Required]
        public string DepartmentId { get; set; } = null!;

        [Required]
        public string Department { get; set; } = null!;
    }
}
