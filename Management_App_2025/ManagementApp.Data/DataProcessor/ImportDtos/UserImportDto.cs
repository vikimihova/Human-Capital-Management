using System.ComponentModel.DataAnnotations;

using static ManagementApp.Common.EntityValidationConstants.UserValidationConstants;
using static ManagementApp.Common.EntityValidationConstants.DepartmentValidationConstants;
using static ManagementApp.Common.EntityValidationConstants.JobTitleValidationConstants;

namespace ManagementApp.Data.DataProcessor.ImportDtos
{
    public class UserImportDto
    {
        [Required]
        [MinLength(UserFirstNameMinLength)]
        [MaxLength(UserFirstNameMaxLength)]
        public string FirstName{ get; set; } = null!;

        [Required]
        [MinLength(UserLastNameMinLength)]
        [MaxLength(UserLastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [MinLength(UsernameMinLength)]
        [MaxLength(UsernameMaxLength)]
        public string Username { get; set; } = null!;

        [Required]
        [MinLength(UserPasswordMinLength)]
        [MaxLength(UserPasswordMaxLength)]
        public string Password { get; set; } = null!;

        [Required]
        [MinLength(UserEmailMinLength)]
        [MaxLength(UserEmailMaxLength)]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(JobTitleNameMinLength)]
        [MaxLength(JobTitleNameMaxLength)]
        public string JobTitle { get; set; } = null!;

        [Required]
        public Guid JobTitleId { get; set; }

        [Required]
        [MinLength(DepartmentNameMinLength)]
        [MaxLength(DepartmentNameMaxLength)]
        public string Department { get; set; } = null!;

        [Required]
        public Guid DepartmentId { get; set; }

        [Required]
        [Range(SalaryMinAmount, SalaryMaxAmount)]
        public decimal Salary { get; set; }

        [Required]
        [MinLength(1)]
        public string Role { get; set; } = null!;
    }
}
