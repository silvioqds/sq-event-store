using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQEventStoreDB.Domain.Events.BankAccount
{
    public record AccountWithDrawnEvent(Guid AccountId, decimal Amount, string Currency) : BaseEvent(AccountId);
}
