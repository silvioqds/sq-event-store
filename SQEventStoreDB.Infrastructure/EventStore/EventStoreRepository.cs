using EventStore.Client;
using System.Text.Json;
using System.Text;
using SQEventStoreDB.Domain.Aggregate;
using SQEventStore.Contracts.Common;
using SQEventStore.Contracts.EventStoreRepository;

namespace SQEventStoreDB.Infrastructure.EventRepository
{
    public class EventStoreRepository<T> : IEventStoreRepository<T> where T : AggregateRoot, new()
    {
        private readonly EventStoreClient _client;

        public EventStoreRepository(EventStoreClient client)
        {
            _client = client;
        }

        public async Task<T> GetByIdAsync(string streamId, CancellationToken cancellationToken = default)
        {
            var aggregate = new T();
            var events = _client.ReadStreamAsync(Direction.Forwards, streamId, StreamPosition.Start, cancellationToken: cancellationToken);

            if (await events.ReadState == ReadState.StreamNotFound)
                return null;

            var domainEvents = new List<IEvent>();

            await foreach (var resolvedEvent in events)
            {
                var json = Encoding.UTF8.GetString(resolvedEvent.Event.Data.Span);
                var eventType = Type.GetType(resolvedEvent.Event.EventType);
                var @event = (IEvent)JsonSerializer.Deserialize(json, eventType);
                domainEvents.Add(@event);
            }

            aggregate.LoadsFromHistory(domainEvents);
            return aggregate;
        }

        public async Task SaveAsync(T aggregate, CancellationToken cancellationToken = default)
        {
            var streamId = $"{typeof(T).Name}-{aggregate.Id}";
            var events = aggregate.GetUncommittedEvents()
                .Select(@event =>
                    new EventData(
                        Uuid.NewUuid(),
                        @event.GetType().FullName,
                        JsonSerializer.SerializeToUtf8Bytes(@event)
                    )
                ).ToArray();

            if (events.Length == 0)
                return;

            var expectedVersion = aggregate.Version == -1 ? StreamState.NoStream : StreamState.StreamExists;

            await _client.AppendToStreamAsync(streamId, expectedVersion, events, cancellationToken: cancellationToken);

            aggregate.MarkEventsAsCommitted();
        }
    }
}
