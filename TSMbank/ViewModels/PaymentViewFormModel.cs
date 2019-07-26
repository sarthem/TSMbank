using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.ViewModels
{
    public class PaymentViewFormModel
    {
        // Transaction
        public Individual Individual { get; set; }
        public List<BankAccount> BankAccounts { get; set; }
        public string BankAccountId { get; set; }
        public string CreditAccount { get; set; }
        public decimal Amount { get; set; }
        public List<BankAccount> CreditIBAN { get; set; }
        public TransactionCategory Category { get; set; }

        // My Bill
        public int Id { get; set; }
        public int OnlinePaymentCode { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Comments { get; set; }

        public BankAccount PublicServiceType { get; set; }


    }
}