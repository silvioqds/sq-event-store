﻿using SQEventStore.Contracts.Common;

namespace SQEventStoreDB.Domain.Events.BankAccount
{
    public record BankAccountOpenedEvent(Guid AccountId, Guid OwnerId) : IEvent
    {
        public DateTime OccurredOn => DateTime.UtcNow;
    }
}
