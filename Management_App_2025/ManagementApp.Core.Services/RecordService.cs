using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.ApplicationRole;
using ManagementApp.Core.ViewModels.ApplicationUser;
using ManagementApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using static ManagementApp.Common.ApplicationConstants;
using static ManagementApp.Common.ErrorMessages.Roles;
using static ManagementApp.Common.ErrorMessages.UserCreation;

namespace ManagementApp.Core.Services
{
    public class RecordService : BaseService, IRecordService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStore<ApplicationUser> userStore;
        private readonly RoleManager<ApplicationRole> roleManager;

        public RecordService(UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.userStore = userStore;
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
                    Id = user.Id.ToString(),
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

            model = model
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToList();

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

        public async Task<bool> AddRecordAsync(AddRecordInputModel model)
        {
            // check if guids are valid
            Guid departmentGuid = Guid.Empty;
            Guid jobTitleGuid = Guid.Empty;
            if (!IsGuidValid(model.DepartmentId, ref departmentGuid) || 
                !IsGuidValid(model.JobTitleId, ref jobTitleGuid))
            {
                throw new ArgumentException();
            }

            // check if role is valid
            ApplicationRole? role = await this.roleManager
                .Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == model.RoleName);

            if (role == null)
            {
                throw new InvalidOperationException();
            }

            // check if user already exists
            ApplicationUser? user = await this.userManager
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.FirstName == model.FirstName &&
                                          u.LastName == model.LastName);

            if (user != null)
            {
                throw new InvalidOperationException();
            }

            user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Salary = model.Salary,
                DepartmentId = departmentGuid,
                JobTitleId = jobTitleGuid
            };

            // set username
            await userStore.SetUserNameAsync(user, model.Username, CancellationToken.None);

            // register user and set password
            IdentityResult result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(String.Format(ErrorWhileRegisteringUser, model.Username));
            }

            // assign user to role
            IdentityResult userRoleResult = await userManager.AddToRoleAsync(user, model.RoleName);

            if (!userRoleResult.Succeeded)
            {
                throw new InvalidOperationException(String.Format(ErrorWhileAddingUserToRole, model.Username, model.RoleName));
            }

            return true;
        }

        public async Task<bool> EditRecordByAdminAsync(EditRecordInputModel model)
        {
            // check if guids are valid
            Guid departmentGuid = Guid.Empty;
            Guid jobTitleGuid = Guid.Empty;
            if (!IsGuidValid(model.DepartmentId, ref departmentGuid) ||
                !IsGuidValid(model.JobTitleId, ref jobTitleGuid))
            {
                throw new ArgumentException();
            }

            //// check if role is valid
            //ApplicationRole? role = await this.roleManager
            //    .Roles
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(r => r.Name == model.RoleName);

            //if (role == null)
            //{
            //    throw new InvalidOperationException();
            //}

            // check if user already exists
            ApplicationUser? user = await this.userManager
                .Users
                .FirstOrDefaultAsync(u => u.Id.ToString() == model.Id);

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Salary = model.Salary;
            user.DepartmentId = departmentGuid;
            user.JobTitleId = jobTitleGuid;

            //bool isInRole = await this.userManager.IsInRoleAsync(user, model.RoleName);

            //if (isInRole == false)
            //{
            //    IdentityResult userRoleResult = await this.userManager.AddToRoleAsync(user, model.RoleName);

            //    if (!userRoleResult.Succeeded)
            //    {
            //        throw new InvalidOperationException();
            //    }
            //}

            await this.userManager.UpdateAsync(user);

            return true;
        }

        public async Task<bool> EditRecordByManagerAsync(EditRecordInputModel model)
        {
            // check if guids are valid
            Guid departmentGuid = Guid.Empty;
            Guid jobTitleGuid = Guid.Empty;
            if (!IsGuidValid(model.DepartmentId, ref departmentGuid) ||
                !IsGuidValid(model.JobTitleId, ref jobTitleGuid))
            {
                throw new ArgumentException();
            }

            // check if user already exists
            ApplicationUser? user = await this.userManager
                .Users
                .FirstOrDefaultAsync(u => u.Id.ToString() == model.Id);

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            user.Salary = model.Salary;
            user.JobTitleId = jobTitleGuid;
            await this.userManager.UpdateAsync(user);

            return true;
        }

        public async Task<bool> DeleteRecordAsync(string userId)
        {
            // check input
            Guid userGuid = Guid.Empty;
            if (!IsGuidValid(userId, ref userGuid))
            {
                throw new ArgumentException();
            }

            // check if user exists
            ApplicationUser? user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            // check if user is already deleted
            if (user.IsDeleted == true)
            {
                return false;
            }

            // soft delete user
            user.IsDeleted = true;
            await this.userManager.UpdateAsync(user);

            return true;
        }        

        // AUXILIARY

        public async Task<EditRecordInputModel> GenerateEditRecordInputModelAsync(string userId)
        {
            // check input
            Guid userGuid = Guid.Empty;
            if (!IsGuidValid(userId, ref userGuid))
            {
                throw new ArgumentException();
            }

            // check if user exists
            ApplicationUser? user = await this.userManager.FindByIdAsync(userId);

            if (user == null || user.IsDeleted == true)
            {
                throw new InvalidOperationException();
            }

            EditRecordInputModel model = new EditRecordInputModel()
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Salary = user.Salary,
                DepartmentId = user.DepartmentId.ToString(),
                JobTitleId = user.JobTitleId.ToString()
            };

            return model;
        }

        public async Task<IEnumerable<SelectRoleViewModel>> GetRolesAsync()
        {
            IEnumerable<SelectRoleViewModel> roles = await roleManager
                .Roles
                .AsNoTracking()
                .Select(r => new SelectRoleViewModel()
                {
                    Id = r.Id.ToString(),
                    Name = r.Name!
                })
                .OrderBy(r => r.Name)
                .ToArrayAsync();

            return roles;
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
