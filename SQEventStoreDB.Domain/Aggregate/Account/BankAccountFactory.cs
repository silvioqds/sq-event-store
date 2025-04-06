using SQEventStore.Contracts.Abstractions.Domain;
using SQEventStore.Contracts.Factories;
using SQEventStoreDB.Domain.Aggregate.Account;

namespace SQEventStoreDB.Domain.Aggregate
{
    public class BankAccountFactory : IBankAccountFactory
    {
        public IBankAccount Create(Guid accountId, Guid ownerId) => new BankAccount(accountId, ownerId);
    }
}
