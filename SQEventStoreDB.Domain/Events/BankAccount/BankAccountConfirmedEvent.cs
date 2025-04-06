using SQEventStore.Contracts.Common;

namespace SQEventStoreDB.Domain.Events.BankAccount
{

    public record BankAccountConfirmedEvent(Guid AccountId, DateTime ConfirmedAt) : IEvent
    {
        public DateTime OccurredOn => DateTime.UtcNow;
    }
}
