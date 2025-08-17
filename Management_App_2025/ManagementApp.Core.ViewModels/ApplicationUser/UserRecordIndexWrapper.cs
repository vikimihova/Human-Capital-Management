using ManagementApp.Core.ViewModels.Department;
using ManagementApp.Core.ViewModels.JobTitle;

namespace ManagementApp.Core.ViewModels.ApplicationUser
{
    public class UserRecordIndexWrapper
    {
        public ICollection<UserRecordViewModel> Users { get; set; } = new List<UserRecordViewModel>();

        public IEnumerable<SelectJobTitleViewModel> JobTitles { get; set; } = new List<SelectJobTitleViewModel>();

        public IEnumerable<SelectDepartmentViewModel> Departments { get; set; } = new List<SelectDepartmentViewModel>();

        public string? SearchInput { get; set; }

        public string? DepartmentFilter { get; set; }

        public string? JobTitleFilter { get; set; }
    }
}
