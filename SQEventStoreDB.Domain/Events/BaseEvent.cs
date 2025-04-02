using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQEventStoreDB.Domain.Events
{
    public abstract record BaseEvent(Guid EventId)
    { 
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    }
}
