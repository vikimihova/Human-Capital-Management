using Microsoft.EntityFrameworkCore;
using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.JobTitle;
using ManagementApp.Data.Models;
using ManagementApp.Data.Repository.Interfaces;
using ManagementApp.Common.CustomExceptions;

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
            IEnumerable<JobTitleViewModel> model = await this.jobTitleRepository
                .GetAllAttached()
                .AsNoTracking()
                .Include(j => j.ApplicationUsers)
                .Select(j => new JobTitleViewModel()
                {
                    Id = j.Id.ToString(),
                    Name = j.Name,
                    EmployeesCount = j.ApplicationUsers.Count,
                    IsDeleted = j.IsDeleted
                })
                .OrderBy(j => j.Name)
                .ToArrayAsync();

            return model;
        }

        public async Task<bool> AddJobTitleAsync(AddJobTitleInputModel model)
        {          
            // check if jobTitle already exists
            JobTitle? jobTitle = await this.jobTitleRepository
                .GetAllAttached()
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Name == model.Name);

            if (jobTitle != null)
            {
                throw new InvalidOperationException();
            }

            jobTitle = new JobTitle()
            {
                Name = model.Name
            };

            await this.jobTitleRepository.AddAsync(jobTitle);

            return true;
        }

        public async Task<bool> EditJobTitleAsync(EditJobTitleInputModel model)
        {
            // check if guids are valid
            Guid jobTitleGuid = Guid.Empty;
            if (!IsGuidValid(model.Id, ref jobTitleGuid))
            {
                throw new ArgumentException();
            }

            // check if jobTitle already exists
            JobTitle? jobTitle = await this.jobTitleRepository
                .FirstOrDefaultAsync(j => j.Id == jobTitleGuid);

            if (jobTitle == null)
            {
                throw new InvalidOperationException();
            }

            jobTitle.Name = model.Name;            

            await this.jobTitleRepository.UpdateAsync(jobTitle);

            return true;
        }

        public async Task<bool> DeleteJobTitleAsync(string id)
        {
            // check input
            Guid jobTitleGuid = Guid.Empty;
            if (!IsGuidValid(id, ref jobTitleGuid))
            {
                throw new ArgumentException();
            }

            // check if jobTitle exists
            JobTitle? jobTitle = await this.jobTitleRepository
                .GetAllAttached()
                .Include(j => j.ApplicationUsers)
                .FirstOrDefaultAsync(j => j.Id == jobTitleGuid);

            if (jobTitle == null)
            {
                return false;
            }

            // check if jobTitle already soft deleted
            if (jobTitle.IsDeleted == false)
            {
                return false;
            }

            // check if jobTitle has no employees
            if (jobTitle.ApplicationUsers.Any())
            {
                return false;
            }

            // hard delete jobTitle
            await this.jobTitleRepository.DeleteAsync(jobTitle);

            return true;
        }

        public async Task<bool> SoftDeleteJobTitleAsync(string id)
        {
            // check input
            Guid jobTitleGuid = Guid.Empty;
            if (!IsGuidValid(id, ref jobTitleGuid))
            {
                throw new ArgumentException();
            }

            // check if jobTitle exists
            JobTitle? jobTitle = await this.jobTitleRepository
                .GetAllAttached()
                .Include(j => j.ApplicationUsers)
                .FirstOrDefaultAsync(j => j.Id == jobTitleGuid);

            if (jobTitle == null)
            {
                return false;
            }

            // check if jobTitle already soft deleted
            if (jobTitle.IsDeleted == true)
            {
                return false;
            }

            // check if jobTitle has no employees
            if (jobTitle.ApplicationUsers.Any(u => u.IsDeleted == false))
            {
                return false;
            }

            // soft delete jobTitle
            jobTitle.IsDeleted = true;
            await this.jobTitleRepository.UpdateAsync(jobTitle);

            return true;
        }

        public async Task<bool> IncludeJobTitleAsync(string id)
        {
            // check input
            Guid jobTitleGuid = Guid.Empty;
            if (!IsGuidValid(id, ref jobTitleGuid))
            {
                throw new ArgumentException();
            }

            // check if jobTitle exists
            JobTitle? jobTitle = await this.jobTitleRepository
                .FirstOrDefaultAsync(j => j.Id == jobTitleGuid);

            if (jobTitle == null)
            {
                return false;
            }

            // check if jobTitle already soft deleted
            if (jobTitle.IsDeleted != true)
            {
                return false;
            }

            // include jobTitle
            jobTitle.IsDeleted = false;
            await this.jobTitleRepository.UpdateAsync(jobTitle);

            return true;
        }

        // AUXILIARY

        public async Task<EditJobTitleInputModel> GenerateEditJobTitleInputModelAsync(string id)
        {
            // check input
            Guid jobTitleGuid = Guid.Empty;
            if (!IsGuidValid(id, ref jobTitleGuid))
            {
                throw new ArgumentException();
            }

            // check if jobTitle exists
            JobTitle? jobTitle = await this.jobTitleRepository.GetByIdAsync(jobTitleGuid);

            if (jobTitle == null)
            {
                throw new EntityNullException();
            }

            if (jobTitle.IsDeleted == true)
            {
                throw new InvalidOperationException();
            }

            EditJobTitleInputModel model = new EditJobTitleInputModel()
            {
                Id = id,
                Name = jobTitle.Name
            };

            return model;
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
                .OrderBy(j => j.Name)
                .ToListAsync();

            return model;
        }
    }
}
