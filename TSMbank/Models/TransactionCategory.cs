﻿using System;
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
        Cancellation,
        MoneyTransfer,
        Purchase,
        InterestFee,
        OverdueFee
    }
}