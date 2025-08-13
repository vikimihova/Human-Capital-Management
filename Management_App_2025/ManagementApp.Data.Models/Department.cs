using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using static ManagementApp.Common.EntityValidationConstants.DepartmentValidationConstants;

namespace ManagementApp.Data.Models
{
    public class Department
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(DepartmentNameMaxLength)]
        [Comment("Name of the department")]
        public string Name { get; set; } = null!;

        //[Required]
        //public Guid ManagerId { get; set; }

        //[ForeignKey(nameof(ManagerId))]
        //public ApplicationUser Manager { get; set; } = null!;

        [Required]
        public bool IsDeleted { get; set; } = false;

        public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
    }
}
