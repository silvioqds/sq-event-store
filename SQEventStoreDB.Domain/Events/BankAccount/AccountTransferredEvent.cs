using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQEventStoreDB.Domain.Events.BankAccount
{
    public record AccountTransferredEvent(Guid SourceAccountId, Guid DestinationAccountId, decimal Amount, string Currency) : BaseEvent(SourceAccountId);
}
