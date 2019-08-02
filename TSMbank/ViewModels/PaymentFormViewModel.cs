using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.ViewModels
{
    public class PaymentFormViewModel
    {
        public List<BankAccount> CustomerBankAccs { get; set; }

        [Required]
        public string CustomerBankAccNo { get; set; }

        [Required]
        public string PublicPaymentAccNo { get; set; }

        [Required]
        [Range(0.1, 9999.99, ErrorMessage = "Amount must be between 0.1 and 9999.99")]
        public decimal? Amount { get; set; }
        public List<BankAccount> PublicPaymentAccs { get; set; }

        [Required]
        public TransactionCategory TransactionCategory { get; set; } 

        [Required]
        public string OnlinePaymentCode { get; set; }
    }
}