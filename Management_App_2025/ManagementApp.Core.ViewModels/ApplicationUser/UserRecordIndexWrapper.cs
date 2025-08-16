using ManagementApp.Core.ViewModels.Department;
using ManagementApp.Core.ViewModels.JobTitle;

namespace ManagementApp.Core.ViewModels.ApplicationUser
{
    public class UserRecordIndexWrapper
    {
        IEnumerable<UserRecordViewModel> users { get; set; } = new List<UserRecordViewModel>();

        ICollection<SelectJobTitleViewModel> jobTitles { get; set; } = new List<SelectJobTitleViewModel>();

        ICollection<SelectDepartmentViewModel> departments { get; set; } = new List<SelectDepartmentViewModel>();
    }
}
