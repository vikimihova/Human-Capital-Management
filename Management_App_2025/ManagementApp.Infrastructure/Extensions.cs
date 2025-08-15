using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using ManagementApp.Data;
using ManagementApp.Data.Models;
using ManagementApp.Data.DataProcessor.ImportDtos;

using static ManagementApp.Common.ApplicationConstants;
using static ManagementApp.Common.ErrorMessages.Roles;
using static ManagementApp.Common.ErrorMessages.Services;
using static ManagementApp.Common.ErrorMessages.Seed;
using static ManagementApp.Common.ErrorMessages.UserCreation;

namespace ManagementApp.Infrastructure
{
    public static class Extensions
    {
        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            // create scope
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

            // get dbContext
            ApplicationDbContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>()!;
            context.Database.MigrateAsync();

            return app;
        }

        public static IApplicationBuilder SeedRoles(this IApplicationBuilder app)
        {
            // create scope
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();

            // get role manager
            RoleManager<ApplicationRole>? roleManager = serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();

            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager),
                    String.Format(ErrorTryingToObtainService, typeof(RoleManager<ApplicationRole>)));
            }            

            Task.Run(async () =>
            {
                // check if role Employee already exists and create if necessary
                bool employeeRoleExists = await roleManager.RoleExistsAsync(EmployeeRoleName);

                if (!employeeRoleExists)
                {
                    ApplicationRole employeeRole = new ApplicationRole(EmployeeRoleName);

                    IdentityResult result = await roleManager.CreateAsync(employeeRole);

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(String.Format(ErrorWhileCreatingRole, EmployeeRoleName));
                    }
                }

                // check if role Manager already exists and create if necessary
                bool managerRoleExists = await roleManager.RoleExistsAsync(ManagerRoleName);

                if (!managerRoleExists)
                {
                    ApplicationRole managerRole = new ApplicationRole(ManagerRoleName);

                    IdentityResult result = await roleManager.CreateAsync(managerRole);

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(String.Format(ErrorWhileCreatingRole, ManagerRoleName));
                    }
                }

                // check if role Admin already exists and create if necessary
                bool adminRoleExists = await roleManager.RoleExistsAsync(AdminRoleName);

                if (!adminRoleExists)
                {
                    ApplicationRole adminRole = new ApplicationRole(AdminRoleName);

                    IdentityResult result = await roleManager.CreateAsync(adminRole);

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(String.Format(ErrorWhileCreatingRole, AdminRoleName));
                    }
                }
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }

        public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

            ApplicationDbContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>()!;

            // seed departments and job titles
            context.SeedDepartments();
            context.SeedJobTitles();

            // generate user import dtos
            List<UserImportDto> userImportDtos = context.GenerateUserDtosList();                       

            try
            {
                // get user store
                IUserStore<ApplicationUser>? userStore = serviceScope
                    .ServiceProvider
                    .GetService<IUserStore<ApplicationUser>>();

                if (userStore == null)
                {
                    throw new ArgumentNullException(nameof(userStore),
                        String.Format(ErrorTryingToObtainService, typeof(IUserStore<ApplicationUser>)));
                }

                // get user manager
                UserManager<ApplicationUser>? userManager = serviceScope
                    .ServiceProvider
                    .GetService<UserManager<ApplicationUser>>();

                if (userManager == null)
                {
                    throw new ArgumentNullException(nameof(userManager),
                        String.Format(ErrorTryingToObtainService, typeof(UserManager<ApplicationUser>)));
                }                

                Task.Run(async () =>
                {
                    foreach (var userDto in userImportDtos)
                    {
                        // check if user exists
                        ApplicationUser? user = await userManager.FindByEmailAsync(userDto.Email);

                        if (user == null)
                        {
                            // create user with email
                            user = new ApplicationUser
                            {
                                FirstName = userDto.FirstName,
                                LastName = userDto.LastName,
                                Email = userDto.Email,
                                JobTitleId = userDto.JobTitleId,
                                DepartmentId = userDto.DepartmentId,
                                Salary = userDto.Salary
                            };

                            // set username
                            await userStore.SetUserNameAsync(user, userDto.Username, CancellationToken.None);

                            // register user and set password
                            IdentityResult result = await userManager.CreateAsync(user, userDto.Password);

                            if (!result.Succeeded)
                            {
                                throw new InvalidOperationException(String.Format(ErrorWhileRegisteringUser, userDto.Username));
                            }

                            // assign user to role
                            IdentityResult userRoleResult = await userManager.AddToRoleAsync(user, userDto.Role);

                            if (!userRoleResult.Succeeded)
                            {
                                throw new InvalidOperationException(String.Format(ErrorWhileAddingUserToRole, userDto.Username, userDto.Role));
                            }
                        }
                    }
                })
                        .GetAwaiter()
                        .GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ErrorWhileSeeding, ex.Message);
            }

            return app;
        }
    }
}
