using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SQEventStore.Contracts.Abstractions.Domain;
using SQEventStore.Contracts.EventStoreRepository;
using SQEventStoreDB.Domain.Aggregate.Account;
using SQEventStoreDB.Infrastructure.Adapters;
using SQEventStoreDB.Infrastructure.EventRepository;

namespace SQEventStoreDB.Infrastructure
{
    public static class DependencyInjection
    {

        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var eventStoreConnectionString = configuration.GetSection("EventStore").Value;

            if (!string.IsNullOrEmpty(eventStoreConnectionString))
                services.AddEventStoreClient(eventStoreConnectionString);


            services.AddScoped<IEventStoreRepository<BankAccount>, EventStoreRepository<BankAccount>>();
            services.AddScoped<IEventStoreRepository<IBankAccount>, BankAccountRepositoryAdapter>();            
        }
   }
}
