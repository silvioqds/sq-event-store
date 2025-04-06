using SQEventStore.Contracts.Abstractions.Domain;
using SQEventStore.Contracts.EventStoreRepository;
using SQEventStoreDB.Domain.Aggregate.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQEventStoreDB.Infrastructure.Adapters
{
    public class BankAccountRepositoryAdapter : IEventStoreRepository<IBankAccount>
    {
        private readonly IEventStoreRepository<BankAccount> _inner;

        public BankAccountRepositoryAdapter(IEventStoreRepository<BankAccount> inner)
        {
            _inner = inner;
        }

        public async Task<IBankAccount> GetByIdAsync(string streamId, CancellationToken cancellationToken = default)
        {
            return await _inner.GetByIdAsync(streamId, cancellationToken);
        }

        public Task SaveAsync(IBankAccount aggregate, CancellationToken cancellationToken = default)
        {
            return _inner.SaveAsync((BankAccount)aggregate, cancellationToken);
        }
    }
}
