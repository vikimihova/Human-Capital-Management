using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using ManagementApp.Data.Models;
using ManagementApp.Data.DataProcessor;
using ManagementApp.Data.DataProcessor.ImportDtos;

using static ManagementApp.Common.ErrorMessages.Seed;

namespace ManagementApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<JobTitle> JobTitles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public void SeedDepartments()
        {
            try
            {
                DepartmentImportDto[] departmentImportDtos = Deserializer.GenerateDepartmentImportDtos();

                List<Department> generatedDepartments = new List<Department>();

                foreach (var departmentDto in departmentImportDtos)
                {
                    if (!generatedDepartments.Any(gd => gd.Name == departmentDto.Department))
                    {
                        Department department = new Department()
                        {
                            Name = departmentDto.Department,
                        };

                        generatedDepartments.Add(department);
                    }
                }

                List<Department> existingDepartments = this.Departments
                    .AsNoTracking()
                    .ToList();

                List<Department> newDepartmentsToAdd = generatedDepartments
                    .Where(gd => !existingDepartments.Any(ed => ed.Name == gd.Name))
                    .ToList();

                if (newDepartmentsToAdd.Any())
                {
                    this.Departments.AddRange(newDepartmentsToAdd);
                    this.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ErrorWhileSeeding, ex.Message);
            }
        }

        public void SeedJobTitles()
        {
            try
            {
                JobTitleImportDto[] jobTitleImportDtos = Deserializer.GenerateJobTitleImportDtos();

                List<JobTitle> generatedJobTitles = new List<JobTitle>();

                foreach (var jobTitleDto in jobTitleImportDtos)
                {
                    if (!generatedJobTitles.Any(gj => gj.Name == jobTitleDto.JobTitle))
                    {
                        JobTitle jobTitle = new JobTitle()
                        {
                            Name = jobTitleDto.JobTitle,
                        };

                        generatedJobTitles.Add(jobTitle);
                    }
                }

                List<JobTitle> existingJobTitles = this.JobTitles
                    .AsNoTracking()
                    .ToList();

                List<JobTitle> newJobTitlesToAdd = generatedJobTitles
                    .Where(gj => !existingJobTitles.Any(ej => ej.Name == gj.Name))
                    .ToList();

                if (newJobTitlesToAdd.Any())
                {
                    this.JobTitles.AddRange(newJobTitlesToAdd);
                    this.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ErrorWhileSeeding, ex.Message);
            }
        }

        public static List<UserImportDto> GenerateUserDtosList()
        {
            UserImportDto[] userImportDtos = Deserializer.GenerateUserImportDtos();

            return userImportDtos.ToList();
        }
    }
}
