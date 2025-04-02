using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQEventStoreDB.Domain.Events.BankAccount
{

    public record AccountConfirmedEvent(Guid AccountId, DateTime ConfirmedAt) : BaseEvent(AccountId);
}
