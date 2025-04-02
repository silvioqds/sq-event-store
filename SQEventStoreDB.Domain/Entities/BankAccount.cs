using SQEventStoreDB.Domain.Entities;

namespace SQEventStoreDB.Domain.Account
{
    public sealed class BankAccount : IEntity
    {
        public Guid AccountId { get; private set; }
        public Guid OwnerId { get; private set; }
        public decimal Balance { get; private set; }
        public string Currency { get; private set; }
        private readonly List<object> _events = new();

        public IReadOnlyList<object> Events => _events.AsReadOnly();


    }
}
