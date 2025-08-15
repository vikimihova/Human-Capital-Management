using ManagementApp.Data.Models;
using ManagementApp.Data.Repository;
using ManagementApp.Data.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ManagementApp.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterRepositories(this IServiceCollection services, Assembly modelsAssembly)
        {
            // ignore ApplicationUser
            Type[] typesToExclude = new Type[] { typeof(ApplicationUser) };

            // get all other types
            Type[] modelTypes = modelsAssembly.GetTypes()
                .Where(t => !t.IsAbstract
                        && !t.IsInterface
                        && !t.Name.ToLower().EndsWith("attribute")
                        && !typesToExclude.Contains(t))
                .ToArray();

            if (modelTypes.Any())
            {
                // register repository for each type
                foreach (Type type in modelTypes)
                {
                    // get id info
                    PropertyInfo idPropInfo = type.GetProperty("Id")!;

                    // get type of repository interface and class
                    Type repositoryInterface = typeof(IRepository<,>);
                    Type repositoryInstanceType = typeof(BaseRepository<,>);

                    // define repository interface and class construction arguments
                    Type[] constructArgs = new Type[2];
                    constructArgs[0] = type;

                    if (idPropInfo == null)
                    {
                        constructArgs[1] = typeof(object); // for example if it's a composite key
                    }
                    else
                    {
                        constructArgs[1] = idPropInfo.PropertyType;
                    }

                    // create repository interface and class instances with defined construction arguments
                    repositoryInterface = repositoryInterface.MakeGenericType(constructArgs);
                    repositoryInstanceType = repositoryInstanceType.MakeGenericType(constructArgs);

                    // add to services
                    services.AddScoped(repositoryInterface, repositoryInstanceType);
                }
            }
        }
    }
}
