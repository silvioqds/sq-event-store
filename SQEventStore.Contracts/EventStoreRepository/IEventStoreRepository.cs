using SQEventStore.Contracts.Contracts.Domain;

namespace SQEventStore.Contracts.EventStoreRepository
{
    public interface IEventStoreRepository<T> where T : IAggregate
    {
        Task<T> GetByIdAsync(string streamId, CancellationToken cancellationToken = default);
        Task SaveAsync(T aggregate, CancellationToken cancellationToken = default);
    }
}
