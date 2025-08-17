using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.ApplicationUser;
using ManagementApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using static ManagementApp.Common.ApplicationConstants;

namespace ManagementApp.Core.Services
{
    public class RecordService : BaseService, IRecordService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public RecordService(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        // MAIN
        public async Task<ICollection<UserRecordViewModel>> Index(string userId, UserRecordIndexWrapper inputModel)
        {
            // GET ALL USERS
            IEnumerable<ApplicationUser> users = await this.userManager
                .Users
                .AsNoTracking()
                .Include(u => u.Department)
                .Include(u => u.JobTitle)
                .Where(u => u.IsDeleted == false)
                .Where(u => u.Id.ToString() != userId)
                .ToArrayAsync();

            // populate view model
            ICollection<UserRecordViewModel> model = new List<UserRecordViewModel>();

            foreach (ApplicationUser user in users)
            {
                // get roles per user
                ICollection<string> roles = await this.userManager.GetRolesAsync(user);

                // map to view model
                model.Add(new UserRecordViewModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    JobTitleId = user.JobTitle.Id.ToString(),
                    JobTitle = user.JobTitle.Name,
                    DepartmentId = user.Department.Id.ToString(),
                    Department = user.Department.Name,
                    Salary = user.Salary,
                    RoleNames = roles
                });
            }

            // IMPLEMENT FILTERS
            // search filter
            if (!String.IsNullOrWhiteSpace(inputModel.SearchInput))
            {
                // add new column FullName
                if (model.Any(u => u.FirstName.ToLower().Contains(inputModel.SearchInput.ToLower())) ||
                    model.Any(u => u.LastName.ToLower().Contains(inputModel.SearchInput.ToLower())))
                {
                    model = model
                        .Where(u => u.FirstName.ToLower().Contains(inputModel.SearchInput.ToLower()) ||
                                    u.LastName.ToLower().Contains(inputModel.SearchInput.ToLower()))
                        .ToList();
                }               
            }

            // department filter
            if (!String.IsNullOrWhiteSpace(inputModel.DepartmentFilter))
            {
                model = model
                    .Where(u => u.Department == inputModel.DepartmentFilter)
                    .ToList();
            }

            // job title filter
            if (!String.IsNullOrWhiteSpace(inputModel.JobTitleFilter))
            {
                model = model
                    .Where(u => u.JobTitle == inputModel.JobTitleFilter)
                    .ToList();
            }

            return model;
        }

        public async Task<ICollection<UserRecordViewModel>> GetEmployeesByManager(string userId, UserRecordIndexWrapper inputModel)
        {
            // check if string is a valid Guid
            Guid userGuid = Guid.Empty;
            if (!IsGuidValid(userId, ref userGuid))
            {
                throw new ArgumentException();
            }

            // get user
            ApplicationUser? user = await this.userManager
                .Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == userGuid);

            // check if user exists
            if (user == null)
            {
                throw new InvalidOperationException();
            }

            // check if user is in role Manager
            bool isManager = await userManager.IsInRoleAsync(user, ManagerRoleName);
            if (!isManager)
            {
                throw new InvalidOperationException();
            }

            // get user department and set filter
            string departmentName = user.Department.Name;
            inputModel.DepartmentFilter = departmentName;

            // call Index method
            ICollection<UserRecordViewModel> model = await Index(userId, inputModel);

            return model;
        }

        public async Task<UserRecordViewModel> GetUserByIdAsync(string userId)
        {
            // check if string is a valid Guid
            Guid userGuid = Guid.Empty;
            if (!IsGuidValid(userId, ref userGuid))
            {
                throw new ArgumentException();
            }

            // get user
            ApplicationUser? user = await this.userManager
                .Users
                .Include(u => u.Department)
                .Include(u => u.JobTitle)
                .FirstOrDefaultAsync(u => u.Id == userGuid);

            // check if user exists
            if (user == null)
            {
                throw new InvalidOperationException();
            }

            ICollection<string> roles = await this.userManager.GetRolesAsync(user);

            // generate view model
            UserRecordViewModel model = new UserRecordViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JobTitleId = user.JobTitle.Id.ToString(),
                JobTitle = user.JobTitle.Name,
                DepartmentId = user.Department.Id.ToString(),
                Department = user.Department.Name,
                Salary = user.Salary,
                RoleNames = roles
            };

            return model;
        }

        public Task<bool> AddRecordAsync(AddRecordInputModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditRecordAsync(EditRecordInputModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }        

        // AUXILIARY

        public Task<EditRecordInputModel> GenerateEditRecordInputModelAsync(string userId)
        {
            throw new NotImplementedException();
        }               


        public async Task<string> GetDepartmentNameByUserIdAsync(string userId)
        {
            // check if string is a valid Guid
            Guid userGuid = Guid.Empty;
            if (!IsGuidValid(userId, ref userGuid))
            {
                throw new ArgumentException();
            }

            // get user
            ApplicationUser? user = await this.userManager
                .Users
                .Include(u => u.Department)
                .Include(u => u.JobTitle)
                .FirstOrDefaultAsync(u => u.Id == userGuid);

            // check if user exists
            if (user == null)
            {
                throw new InvalidOperationException();
            }

            return user.Department.Name;
        }
    }
}
