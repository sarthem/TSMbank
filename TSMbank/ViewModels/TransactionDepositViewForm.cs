using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.ViewModels
{
    public class TransactionDepositViewForm
    {
        public Individual Individual { get; set; }
        public List<BankAccount> BankAccounts { get; set; }
        public string BankAccountId { get; set; }
        public string CreditAccount { get; set; }
        public decimal Amount { get; set; }
        public string CreditIBAN { get; set; }

    }
}