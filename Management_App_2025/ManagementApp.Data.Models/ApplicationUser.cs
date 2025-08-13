using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ManagementApp.Common.EntityValidationConstants.UserValidationConstants;

namespace ManagementApp.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        [MaxLength(UserFirstNameMaxLength)]
        [Comment("First name of the user")]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(UserLastNameMaxLength)]
        [Comment("Last name of the user")]
        public string LastName { get; set; } = null!;

        [Required]
        public Guid JobTitleId { get; set; }

        [Required]
        [ForeignKey(nameof(JobTitleId))]
        public JobTitle JobTitle { get; set; } = null!;

        [Required]
        [Range(SalaryMinAmount, SalaryMaxAmount)]
        public decimal Salary { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        [Required]
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; } = null!;

        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
