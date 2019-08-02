using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TSMbank.Models;
using TSMbank.Validations;

namespace TSMbank.ViewModels
{
    public class CardReqViewModel
    {
        public IndividualStatus IndividualStatus { get; set; }
        public CardType CardType { get; set; }

        [Required]
        [Range(100, 5000, ErrorMessage = "Credit limit must be between 100 and 5000.")]
        public decimal? CreditLimit { get; set; }

        [Required]
        [Range(100, 4999.99, ErrorMessage = "Transactions amount limit must be between 100 and 4999.99")]
        [ValidCardTransLimit]
        public decimal? TransactionAmountLimit { get; set; }
    }
}