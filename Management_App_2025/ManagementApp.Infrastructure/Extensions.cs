using ManagementApp.Data;
using ManagementApp.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using static ManagementApp.Common.ApplicationConstants;
using static ManagementApp.Common.ErrorMessages.Roles;
using static ManagementApp.Common.ErrorMessages.Services;

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
    }
}
