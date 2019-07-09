using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.ViewModels
{
    public class TransactionDepositViewForm
    {
        public Customer Customer { get; set; }
        public List<BankAccount> Accounts { get; set; }
        public string AccountId { get; set; }
        public string DebitAccount { get; set; }
        public decimal Amount { get; set; }
        public string DebitIBAN { get; set; }

    }
}