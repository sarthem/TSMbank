using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.ViewModels
{
    public class PaymentFormViewModel
    {
        public List<BankAccount> CustomerBankAccs { get; set; }
        public string CustomerBankAccNo { get; set; }
        public string PublicPaymentAccNo { get; set; }
        public decimal Amount { get; set; }
        public List<BankAccount> PublicPaymentAccs { get; set; } 
        public TransactionCategory TransactionCategory { get; set; } 
        public string OnlinePaymentCode { get; set; }
    }
}