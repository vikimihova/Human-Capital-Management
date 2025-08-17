namespace ManagementApp.Core.ViewModels.JobTitle
{
    public class JobTitleViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int EmployeesCount { get; set; }

        public bool IsDeleted { get; set; }
    }
}
