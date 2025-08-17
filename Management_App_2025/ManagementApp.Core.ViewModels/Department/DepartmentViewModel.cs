using System.Globalization;

namespace ManagementApp.Core.ViewModels.Department
{
    public class DepartmentViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int EmployeesCount {  get; set; }

        public bool IsDeleted { get; set; }
    }
}
