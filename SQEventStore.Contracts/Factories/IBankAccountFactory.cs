using SQEventStore.Contracts.Abstractions.Domain;

namespace SQEventStore.Contracts.Factories
{
    public interface IBankAccountFactory
    {
        IBankAccount Create(Guid accountId, Guid ownerId);
    }
}
