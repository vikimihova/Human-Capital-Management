using ManagementApp.Core.ViewModels.Department;
using ManagementApp.Core.ViewModels.JobTitle;
using System.ComponentModel.DataAnnotations;
using static ManagementApp.Common.EntityValidationConstants.UserValidationConstants;


namespace ManagementApp.Core.ViewModels.ApplicationUser
{
    public class EditRecordInputModel
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

        public IEnumerable<SelectJobTitleViewModel> JobTitles { get; set; } = new List<SelectJobTitleViewModel>();

        [Required]
        [Range(SalaryMinAmount, SalaryMaxAmount)]
        public int Salary { get; set; }

        [Required]
        public string DepartmentId { get; set; } = null!;

        public IEnumerable<SelectDepartmentViewModel> Departments { get; set; } = new List<SelectDepartmentViewModel>();
    }
}
