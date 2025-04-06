using SQEventStore.Contracts.Abstractions.Domain;
using SQEventStoreDB.Domain.Events.BankAccount;

namespace SQEventStoreDB.Domain.Aggregate.Account
{
    public sealed class BankAccount : AggregateRoot, IBankAccount
    {
        public override string Id { get; protected set; }
        public Guid AccountId { get; private set; }
        public Guid OwnerId { get; private set; }
        public decimal Balance { get; private set; }
        public string Currency { get; private set; }


        public BankAccount() { }
        public BankAccount(Guid accountId, Guid ownerId)
        {
            Id = accountId.ToString();
            ApplyChange(new BankAccountOpenedEvent(accountId, ownerId));
        }

        public void Confirmedd(DateTime confirmedAt)
        {
            if (confirmedAt > DateTime.UtcNow)
                throw new InvalidOperationException("Cannot confirm account in the future.");

            ApplyChange(new BankAccountConfirmedEvent(AccountId, confirmedAt));
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be positive.");

            ApplyChange(new BankAccountDepositedEvent(AccountId, amount));
        }

        public void WithDraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdraw amount must be positive.");
            if (Balance < amount)
                throw new InvalidOperationException("Insufficient funds.");

            ApplyChange(new BankAccountWithDrawnEvent(AccountId, amount));
        }

        public void Transferred(decimal amount, Guid destinationAccountId)
        {
            if (Balance < amount)
                throw new InvalidOperationException("Insufficient funds.");
            Balance -= amount;
            ApplyChange(new BankAccountWithDrawnEvent(AccountId, amount));
            ApplyChange(new BankAccountDepositedEvent(destinationAccountId, amount));
        }

        private void CloseAccount()
        {
            if (Balance > 0)
                throw new InvalidOperationException("Cannot close account with positive balance.");
            ApplyChange(new BankAccountClosedEvent(AccountId));
        }

        private void Apply(BankAccountOpenedEvent e)
        {
            AccountId = e.AccountId;
            OwnerId = e.OwnerId;
            Balance = 0;
            Currency = "BRL";
        }

        private void Apply(BankAccountDepositedEvent e)
        {
            Balance += e.Amount;
        }

        private void Apply(BankAccountWithDrawnEvent e)
        {
            Balance -= e.Amount;
        }

        private void Apply(BankAccountClosedEvent e)
        {
            if (Balance > 0)
                throw new InvalidOperationException("Cannot close account with positive balance.");

            AccountId = e.AccountId;
            Balance = 0;
            Currency = string.Empty;
        }

        public new IReadOnlyCollection<SQEventStore.Contracts.Common.IEvent> GetUncommittedEvents()
        {
            return base.GetUncommittedEvents().Cast<SQEventStore.Contracts.Common.IEvent>().ToList().AsReadOnly();
        }
    }
}
