using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.ViewModels
{
    public class TransferMoneyViewModel
    {
        public IEnumerable<BankAccount> CustomerBankAccs { get; set; }

        [Required]
        public string DebitAccNo { get; set; }

        [CreditAccIsRequiredForMoneyTransfer]
        [RegularExpression("^[0-9]{16}$", ErrorMessage = "Account number is not in the correct format.")]
        public string CreditAccNo { get; set; }

        [Required]
        [Range(0.01, 999999999999.9, ErrorMessage = "Amount must be at least 0.01.")]
        public decimal? Amount { get; set; }

        [RegularExpression(@"GR\d{24}", ErrorMessage = "IBAN number is not in the correct format.")]
        public string CreditAccIban { get; set; }

        public TransactionCategory TransactionCategory { get; set; }
        public string ErrorMessage { get; set; }
    }
}