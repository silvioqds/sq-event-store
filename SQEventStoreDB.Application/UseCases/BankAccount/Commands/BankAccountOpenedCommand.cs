using MediatR;
using SQEventStoreDB.Application.DTO;

namespace SQEventStoreDB.Application.UseCases.BankAccount.Commands
{
    public class BankAccountOpenedCommand : IRequest<BankAccountDTO>
    {
        public Guid AccountId { get; }
        public Guid OwnerId { get; }
        public BankAccountOpenedCommand(Guid accountId, Guid ownerId)
        {
            AccountId = accountId;
            OwnerId = ownerId;
        }
    }
}
