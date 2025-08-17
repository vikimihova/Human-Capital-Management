namespace ManagementApp.Core.ViewModels.ApplicationUser
{
    public class UserRecordViewModel
    {
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Department { get; set; } = null!;

        public string DepartmentId { get; set; } = null!;

        public string JobTitle { get; set; } = null!;

        public string JobTitleId { get; set; } = null!;

        public decimal Salary { get; set; }

        public ICollection<string> RoleNames { get; set; } = new List<string>();
    }
}
