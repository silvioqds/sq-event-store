using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SQEventStoreDB.Application;

namespace SQEventStoreDB.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static void AddExternalDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);
        }
    }
}
