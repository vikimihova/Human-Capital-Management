using ManagementApp.Core.ViewModels.ApplicationRole;
using ManagementApp.Core.ViewModels.ApplicationUser;

namespace ManagementApp.Core.Services.Interfaces
{
    public interface IRecordService
    {
        // MAIN
        Task<ICollection<UserRecordViewModel>> Index(string userId, UserRecordIndexWrapper inputModel);

        Task<ICollection<UserRecordViewModel>> GetEmployeesByManager(string userId, UserRecordIndexWrapper inputModel);

        Task<UserRecordViewModel> GetUserByIdAsync(string userId);

        Task<bool> AddRecordAsync(AddRecordInputModel model);

        Task<bool> EditRecordByAdminAsync(EditRecordInputModel model);        

        Task<bool> EditRecordByManagerAsync(EditRecordInputModel model);        

        Task<bool> SoftDeleteRecordAsync(string userId);

        Task<bool> DeleteRecordAsync(string userId);


        // AUXILIARY

        Task<EditRecordInputModel> GenerateEditRecordInputModelAsync(string userId);

        Task<string> GetDepartmentNameByUserIdAsync(string userId);

        Task<IEnumerable<SelectRoleViewModel>> GetRolesAsync();
    }
}
