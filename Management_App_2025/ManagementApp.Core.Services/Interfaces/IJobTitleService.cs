using ManagementApp.Core.ViewModels.JobTitle;

namespace ManagementApp.Core.Services.Interfaces
{
    public interface IJobTitleService
    {
        //MAIN
        Task<IEnumerable<JobTitleViewModel>> Index();

        Task<bool> AddJobTitleAsync(AddJobTitleInputModel model);

        Task<bool> EditJobTitleAsync(EditJobTitleInputModel model);

        Task<bool> SoftDeleteJobTitleAsync(string id);

        Task<bool> DeleteJobTitleAsync(string id);

        Task<bool> IncludeJobTitleAsync(string id);


        // AUXILIARY
        Task<EditJobTitleInputModel> GenerateEditJobTitleInputModelAsync(string id);

        Task<ICollection<SelectJobTitleViewModel>> GetJobTitlesAsync(string? departmentName = null);
    }
}
