using MediatR;
using SQEventStore.Contracts.Abstractions.Domain;
using SQEventStore.Contracts.EventStoreRepository;
using SQEventStore.Contracts.Factories;
using SQEventStoreDB.Application.DTO;
using SQEventStoreDB.Application.UseCases.BankAccount.Commands;
using SQEventStoreDB.Domain.Aggregate.Account;


namespace SQEventStoreDB.Application.UseCases.BankAccount.Handlers
{
    public class BankAccountOpenedHandler : IRequestHandler<BankAccountOpenedCommand, BankAccountDTO>
    {  
        private readonly IEventStoreRepository<IBankAccount> _eventStoreRepository;
        private readonly IBankAccountFactory _bankAccountFactory;    
        public BankAccountOpenedHandler(IEventStoreRepository<IBankAccount> eventStoreRepository, IBankAccountFactory bankAccountFactory)
        {
            _eventStoreRepository = eventStoreRepository;
            _bankAccountFactory = bankAccountFactory;
        }

        public async Task<BankAccountDTO> Handle(BankAccountOpenedCommand request, CancellationToken cancellationToken)
        {
            if (request.OwnerId == Guid.Empty || request.AccountId == Guid.Empty) throw new ArgumentException("OwnerId and AccountId cannot be empty GUIDs");

            var bankAccount = _bankAccountFactory.Create(request.AccountId, request.OwnerId);   

            await _eventStoreRepository.SaveAsync(bankAccount, cancellationToken);

            return new BankAccountDTO
            {
                Id = Guid.NewGuid().ToString(),
                AccountId = bankAccount.AccountId.ToString(),
                OwnerId = bankAccount.OwnerId.ToString(),
                Balance = bankAccount.Balance,
                Currency = bankAccount.Currency
            };
        }
    }
}
