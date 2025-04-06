using SQEventStore.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQEventStore.Contracts.Contracts.Domain
{
    public interface IAggregate
    {
        string Id { get; }
        int Version { get; }
        IReadOnlyCollection<IEvent> GetUncommittedEvents();
        void MarkEventsAsCommitted();
    }
}
