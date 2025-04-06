using SQEventStore.Contracts.Common;

namespace SQEventStoreDB.Domain.Events.BankAccount
{
    public record BankAccountTransferredEvent(Guid SourceAccountId, Guid DestinationAccountId, decimal Amount, string Currency) : IEvent
    {
        public DateTime OccurredOn => DateTime.UtcNow;
    }
}
