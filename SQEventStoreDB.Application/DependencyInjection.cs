using Microsoft.Extensions.DependencyInjection;
using SQEventStore.Contracts.Factories;
using SQEventStoreDB.Domain.Aggregate;

namespace SQEventStoreDB.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {            
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddTransient<IBankAccountFactory, BankAccountFactory>();
        }
    }
}
