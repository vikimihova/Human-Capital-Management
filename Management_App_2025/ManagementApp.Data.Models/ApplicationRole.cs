using Microsoft.AspNetCore.Identity;

namespace ManagementApp.Data.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base()
        {

        }

        public ApplicationRole(string roleName) : base(roleName)
        {

        }
    }
}
