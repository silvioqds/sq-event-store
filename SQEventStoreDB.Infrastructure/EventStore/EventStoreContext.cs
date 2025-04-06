using EventStore.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQEventStoreDB.Infrastructure.EventStore
{
    public class EventStoreContext
    {
        public EventStoreClient EventStoreClient { get; }
        public EventStoreContext(string connectionString)
        {
            var settings = EventStoreClientSettings.Create(connectionString);
            EventStoreClient = new EventStoreClient(settings);
        }
    }
}
