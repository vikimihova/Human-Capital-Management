using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

using static ManagementApp.Common.EntityValidationConstants.JobTitleValidationConstants;

namespace ManagementApp.Data.Models
{
    public class JobTitle
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(JobTitleNameMaxLength)]
        [Comment("Name of the job title")]
        public string Name { get; set; } = null!;

        [Required]
        public bool IsDeleted { get; set; } = false;

        public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
    }
}
