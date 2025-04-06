using SQEventStore.Contracts.Common;
using SQEventStore.Contracts.Contracts.Domain;
using System.Linq.Expressions;
using System.Reflection;

namespace SQEventStoreDB.Domain.Aggregate
{
    public abstract class AggregateRoot : IAggregate
    {
        private readonly List<IEvent> _uncommittedEvents = new();
        public IReadOnlyCollection<IEvent> GetUncommittedEvents() => _uncommittedEvents.AsReadOnly();
        public void MarkEventsAsCommitted() => _uncommittedEvents.Clear();
        public int Version { get; protected set; } = -1;
        public abstract string Id { get; protected set; }

        // Cache: Type do Aggregate -> Type do Event -> Delegate
        private static readonly Dictionary<Type, Dictionary<Type, Action<object, IEvent>>> _applyMethodCache = new();

        protected void ApplyChange(IEvent @event, bool isNew = true)
        {
            DispatchApply(@event);

            if (isNew)
                _uncommittedEvents.Add(@event);
        }

        public void LoadsFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var @event in history)
            {
                ApplyChange(@event, isNew: false);
                Version++;
            }
        }

        private void DispatchApply(IEvent @event)
        {
            var aggregateType = GetType();
            var eventType = @event.GetType();

            if (!_applyMethodCache.TryGetValue(aggregateType, out var applyMethods))
            {
                applyMethods = new Dictionary<Type, Action<object, IEvent>>();
                _applyMethodCache[aggregateType] = applyMethods;
            }

            if (!applyMethods.TryGetValue(eventType, out var applyDelegate))
            {
                // Busca o método Apply(EventType)
                var methodInfo = aggregateType.GetMethod("Apply", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new[] { eventType }, null);
                if (methodInfo == null)
                    throw new InvalidOperationException($"Método Apply({eventType.Name}) não encontrado em {aggregateType.Name}");

                // Cria delegate (object aggregate, IEvent e) => ((AggregateType)aggregate).Apply((EventType)e)
                var aggregateParam = Expression.Parameter(typeof(object), "aggregate");
                var eventParam = Expression.Parameter(typeof(IEvent), "event");

                var castAggregate = Expression.Convert(aggregateParam, aggregateType);
                var castEvent = Expression.Convert(eventParam, eventType);
                var call = Expression.Call(castAggregate, methodInfo, castEvent);

                applyDelegate = Expression.Lambda<Action<object, IEvent>>(call, aggregateParam, eventParam).Compile();
                applyMethods[eventType] = applyDelegate;
            }

            applyDelegate(this, @event);
        }
    }
}
