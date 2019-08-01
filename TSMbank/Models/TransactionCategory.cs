using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public enum TransactionCategory
    {
        Deposit = 1,
        Withdrawal,
        Payment,
        Cancellation,
        MoneyTransfer,
        Purchase,
        InterestFee,
        OverdueFee,
        Commision
    }
}