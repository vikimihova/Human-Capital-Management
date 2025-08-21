using ManagementApp.Core.ViewModels.Department;

namespace ManagementApp.Core.Services.Interfaces
{
    public interface IDepartmentService
    {
        // MAIN
        Task<IEnumerable<DepartmentViewModel>> Index();

        Task<bool> AddDepartmentAsync(AddDepartmentInputModel model);

        Task<bool> EditDepartmentAsync(EditDepartmentInputModel model);

        Task<bool> DeleteDepartmentAsync(string id);

        Task<bool> IncludeDepartmentAsync(string id);


        // AUXILIARY
        Task<EditDepartmentInputModel> GenerateEditDepartmentInputModelAsync(string id);

        Task<ICollection<SelectDepartmentViewModel>> GetDepartmentsAsync();
    }
}
