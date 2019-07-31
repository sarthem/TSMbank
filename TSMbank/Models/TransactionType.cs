using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public class TransactionType
    {
        public const byte Deposit = 1;
        public const byte Withdrawl = 2;
        public const byte Payment = 3;
        public const byte Cancellation = 4;
        public const byte MoneyTransfer = 5;
        public const byte Purchase = 6;
        public const byte InterestFee = 7;
        public const byte OverdueFee = 8;
        public const byte BankCommission = 9;

        public byte Id { get; set; }
        public TransactionCategory Category { get; set; }
        public decimal Fee { get; set; }
        public string Description { get; set; }
    }
}