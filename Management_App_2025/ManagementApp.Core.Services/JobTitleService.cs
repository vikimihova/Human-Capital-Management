using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.JobTitle;
using ManagementApp.Data.Repository.Interfaces;
using ManagementApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagementApp.Core.Services
{
    public class JobTitleService : BaseService, IJobTitleService
    {
        private readonly IRepository<JobTitle, Guid> jobTitleRepository;

        public JobTitleService(IRepository<JobTitle, Guid> jobTitleRepository)
        {
            this.jobTitleRepository = jobTitleRepository;
        }

        // MAIN

        public async Task<IEnumerable<JobTitleViewModel>> Index()
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddJobTitleAsync(AddJobTitleInputModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditJobTitleAsync(EditJobTitleInputModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteJobTitleAsync(string id)
        {
            throw new NotImplementedException();
        }

        // AUXILIARY

        public Task<EditJobTitleInputModel> GenerateEditJobTitleInputModelAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<SelectJobTitleViewModel>> GetJobTitlesAsync(string? departmentName = null)
        {
            // get all jobs
            IQueryable<JobTitle> jobTitles = this.jobTitleRepository
                .GetAllAttached()
                .AsNoTracking()
                .Include(j => j.ApplicationUsers)
                .ThenInclude(u => u.Department)
                .Where(j => j.IsDeleted == false);

            // filter by department
            if (!String.IsNullOrWhiteSpace(departmentName))
            {
                jobTitles = jobTitles
                    .Where(j => j.ApplicationUsers.Any(u => u.Department.Name == departmentName));
            }

            // create model
            ICollection<SelectJobTitleViewModel> model = await jobTitles
                .Select(j => new SelectJobTitleViewModel()
                {
                    Id = j.Id.ToString(),
                    Name = j.Name,
                })
                .ToListAsync();

            return model;
        }
    }
}
