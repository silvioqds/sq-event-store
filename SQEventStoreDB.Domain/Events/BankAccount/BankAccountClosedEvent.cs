using SQEventStore.Contracts.Common;

namespace SQEventStoreDB.Domain.Events.BankAccount
{
    public record BankAccountClosedEvent(Guid AccountId) : IEvent
    {
        public DateTime OccurredOn => DateTime.UtcNow;
    }
}
