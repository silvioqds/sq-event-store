using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQEventStoreDB.Application.DTO
{
    public class BankAccountDTO
    {
        public string Id { get;  init; }
        public string AccountId { get; init; }
        public string OwnerId { get; init; }
        public decimal Balance { get; init; }
        public string Currency { get; init; }
    }
}
