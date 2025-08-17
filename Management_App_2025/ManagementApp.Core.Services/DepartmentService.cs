using ManagementApp.Core.Services.Interfaces;
using ManagementApp.Core.ViewModels.Department;
using ManagementApp.Data.Models;
using ManagementApp.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

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

        public async Task<bool> AddDepartmentAsync(AddDepartmentInputModel model)
        {
            // check if department already exists
            Department? department = await this.departmentRepository
                .GetAllAttached()
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Name == model.Name);

            if (department != null)
            {
                throw new InvalidOperationException();
            }

            department = new Department()
            {
                Name = model.Name
            };

            await this.departmentRepository.AddAsync(department);

            return true;
        }

        public async Task<bool> EditDepartmentAsync(EditDepartmentInputModel model)
        {
            // check if department already exists
            Department? department = await this.departmentRepository
                .GetAllAttached()
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id.ToString() == model.Id);

            if (department == null)
            {
                throw new InvalidOperationException();
            }

            department.Name = model.Name;

            await this.departmentRepository.UpdateAsync(department);

            return true;
        }

        public async Task<bool> DeleteDepartmentAsync(string id)
        {
            // check input
            Guid departmentGuid = Guid.Empty;
            if (!IsGuidValid(id, ref departmentGuid))
            {
                throw new ArgumentException();
            }

            // check if department exists
            Department? department = await this.departmentRepository.FirstOrDefaultAsync(d => d.Id == departmentGuid);

            if (department == null)
            {
                throw new InvalidOperationException();
            }

            // check if department already deleted
            if (department.IsDeleted == true)
            {
                return false;
            }

            // check if department has no employees
            if (department.ApplicationUsers.Any())
            {
                throw new InvalidOperationException();
            }

            // soft delete department
            department.IsDeleted = true;
            await this.departmentRepository.UpdateAsync(department);

            return true;
        }

        public async Task<bool> IncludeDepartmentAsync(string id)
        {
            // check input
            Guid departmentGuid = Guid.Empty;
            if (!IsGuidValid(id, ref departmentGuid))
            {
                throw new ArgumentException();
            }

            // check if department exists
            Department? department = await this.departmentRepository.FirstOrDefaultAsync(d => d.Id == departmentGuid);

            if (department == null)
            {
                throw new InvalidOperationException();
            }

            // check if department already deleted
            if (department.IsDeleted != true)
            {
                return false;
            }

            // include department
            department.IsDeleted = false;
            await this.departmentRepository.UpdateAsync(department);

            return true;
        }

        // AUXILIARY

        public async Task<EditDepartmentInputModel> GenerateEditDepartmentInputModelAsync(string id)
        {
            // check input
            Guid departmentGuid = Guid.Empty;
            if (!IsGuidValid(id, ref departmentGuid))
            {
                throw new ArgumentException();
            }

            // check if department exists
            Department? department = await this.departmentRepository.GetByIdAsync(departmentGuid);

            if (department == null || department.IsDeleted == true)
            {
                throw new InvalidOperationException();
            }

            EditDepartmentInputModel model = new EditDepartmentInputModel()
            {
                Id = id,
                Name = department.Name
            };

            return model;
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
