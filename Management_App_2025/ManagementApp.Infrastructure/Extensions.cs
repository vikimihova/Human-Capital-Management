using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

using ManagementApp.Data;

namespace ManagementApp.Infrastructure
{
    public static class Extensions
    {
        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

            ApplicationDbContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>()!;
            context.Database.MigrateAsync();

            return app;
        }
    }
}
