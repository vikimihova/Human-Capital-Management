using ManagementApp.Core.ViewModels.ApplicationRole;
using ManagementApp.Core.ViewModels.ApplicationUser;
using ManagementApp.Core.ViewModels.JobTitle;

namespace ManagementApp.Core.Services.Interfaces
{
    public interface IRecordService
    {
        // MAIN
        Task<ICollection<UserRecordViewModel>> Index(string userId, UserRecordIndexWrapper inputModel);

        Task<ICollection<UserRecordViewModel>> GetEmployeesByManager(string userId, UserRecordIndexWrapper inputModel);

        Task<UserRecordViewModel> GetUserByIdAsync(string userId);

        Task<bool> AddRecordAsync(AddRecordInputModel model);

        Task<bool> EditRecordAsync(EditRecordInputModel model);        

        Task<bool> DeleteRecordAsync(string userId);


        // AUXILIARY

        Task<EditRecordInputModel> GenerateEditRecordInputModelAsync(string userId);

        Task<string> GetDepartmentNameByUserIdAsync(string userId);

        Task<IEnumerable<SelectRoleViewModel>> GetRolesAsync();
    }
}
