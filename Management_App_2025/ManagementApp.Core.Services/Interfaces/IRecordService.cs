using ManagementApp.Core.ViewModels.ApplicationUser;
using ManagementApp.Core.ViewModels.JobTitle;

namespace ManagementApp.Core.Services.Interfaces
{
    public interface IRecordService
    {
        // MAIN
        Task<UserRecordIndexWrapper> Index();

        Task<UserRecordViewModel> GetUserByIdAsync(string userId);

        Task<bool> AddRecordAsync(AddRecordInputModel model);

        Task<bool> EditRecordAsync(EditRecordInputModel model);        

        Task<bool> DeleteUserAsync(string userId);


        // AUXILIARY
        Task<EditRecordInputModel> GenerateEditRecordInputModelAsync(string userId);

        Task<bool> AssignUserToRoleAsync(string userId, string roleName);

        Task<bool> RemoveUserFromRoleAsync(string userId, string roleName);
    }
}
