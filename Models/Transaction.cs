using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletService.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Identifier for the user who performed the transaction
        public Guid WalletId { get; set; } // Identifier for the wallet involved in the transaction
        public decimal Amount { get; set; } // Amount of funds involved in the transaction
        public DateTime Timestamp { get; set; } // Timestamp of when the transaction occurred

    }
}
