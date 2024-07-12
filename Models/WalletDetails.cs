using System;
using System.ComponentModel.DataAnnotations;

namespace Models.WalletDetails
{
    public class WalletDetails
    {
         
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
        public string UserId { get; set; } 


        // Field for optimistic concurrency control
        [ConcurrencyCheck]
        public int TransactionCount { get; set; }
    } 

}