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
        public decimal CreditLimit { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        [ValidCardTransLimit]
        public decimal TransactionAmountLimit { get; set; }
    }
}