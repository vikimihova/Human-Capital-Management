using System.ComponentModel.DataAnnotations;

using ManagementApp.Core.ViewModels.ApplicationRole;
using ManagementApp.Core.ViewModels.Department;
using ManagementApp.Core.ViewModels.JobTitle;

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
        public string JobTitleId { get; set; } = null!;

        [Required]
        public IEnumerable<SelectJobTitleViewModel> JobTitles { get; set; } = new List<SelectJobTitleViewModel>();

        [Required]
        [Range(SalaryMinAmount, SalaryMaxAmount)]
        public decimal Salary { get; set; }

        [Required]
        public string DepartmentId { get; set; } = null!;

        [Required]
        public IEnumerable<SelectDepartmentViewModel> Departments { get; set; } = new List<SelectDepartmentViewModel>();

        [Required]
        public string RoleName { get; set; } = null!;

        [Required]
        public IEnumerable<SelectRoleViewModel> Roles { get; set; } = new List<SelectRoleViewModel>();
    }
}
