using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public enum TransactionCategory
    {
        Deposit,
        Withdrawal,
        Payment,
        Bankfee,
        Cancellation
    }

    public class TransactionType
    {
        public byte Id { get; set; }
        public TransactionCategory Category { get; set; }
        public decimal Fee { get; set; }
    }
}