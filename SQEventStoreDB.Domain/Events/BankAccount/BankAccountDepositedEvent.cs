﻿using SQEventStore.Contracts.Common;

namespace SQEventStoreDB.Domain.Events.BankAccount
{
    public record BankAccountDepositedEvent(Guid AccountId, decimal Amount) : IEvent
    {
        public DateTime OccurredOn => DateTime.UtcNow;
    }
}
