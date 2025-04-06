using SQEventStore.Contracts.Contracts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQEventStore.Contracts.Abstractions.Domain
{
    public interface IBankAccount : IAggregate
    {        
        Guid AccountId { get; }
        Guid OwnerId { get; }
        decimal Balance { get; }
        string Currency { get; }
    }
}
