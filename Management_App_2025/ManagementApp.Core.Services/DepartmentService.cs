using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.Department;
using ManagementApp.Data.Models;
using ManagementApp.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManagementApp.Core.Services
{
    public class DepartmentService : BaseService, IDepartmentService
    {
        private readonly IRepository<Department, Guid> departmentRepository;

        public DepartmentService(IRepository<Department, Guid> departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }

        // MAIN
        public async Task<IEnumerable<DepartmentViewModel>> Index()
        {
            IEnumerable<DepartmentViewModel> model = await this.departmentRepository
                .GetAllAttached()
                .AsNoTracking()
                .Include(d => d.ApplicationUsers)
                .Select(d => new DepartmentViewModel()
                {
                    Id = d.Id.ToString(),
                    Name = d.Name,
                    EmployeesCount = d.ApplicationUsers.Count,
                    IsDeleted = d.IsDeleted
                })
                .ToArrayAsync();

            return model;
        }

        public Task<bool> AddDepartmentAsync(AddDepartmentInputModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditDepartmentAsync(EditDepartmentInputModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDepartmentAsync(string id)
        {
            throw new NotImplementedException();
        }

        // AUXILIARY

        public Task<EditDepartmentInputModel> GenerateEditDepartmentInputModelAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<SelectDepartmentViewModel>> GetDepartmentsAsync()
        {
            ICollection<SelectDepartmentViewModel> model = await this.departmentRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(d => d.IsDeleted == false)
                .Select(d => new SelectDepartmentViewModel()
                {
                    Id = d.Id.ToString(),
                    Name = d.Name,
                })
                .ToListAsync();

            return model;
        }
    }
}
