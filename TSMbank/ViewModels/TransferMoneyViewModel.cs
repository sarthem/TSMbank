using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.ViewModels
{
    public class TransferMoneyViewModel
    {
        //public Individual Individual { get; set; }
        //public List<BankAccount> BankAccounts { get; set; }
        //public string BankAccountId { get; set; }
        //public string CreditAccount { get; set; }
        //public decimal Amount { get; set; }
        //public string CreditIBAN { get; set; }
        //public TransactionCategory Category { get; set; }

        public IEnumerable<BankAccount> CustomerBankAccs { get; set; }
        public string DebitAccNo { get; set; }
        public string CreditAccNo { get; set; }
        public decimal Amount { get; set; }
        public string CreditAccIban { get; set; }
        public TransactionCategory TransactionCategory { get; set; }
        public string ErrorMessage { get; set; }
    }
}