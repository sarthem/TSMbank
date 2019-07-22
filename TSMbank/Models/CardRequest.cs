using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TSMbank.Validations;

namespace TSMbank.Models
{
    public class CardRequest : Request
    {
        [Required]
        [Range(100, 5000, ErrorMessage = "Credit limit must be between 100 and 5000.")]
        public decimal CreditLimit { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        [CardTransLimit]
        public decimal TransactionAmountLimit { get; set; }

        public CardType CardType { get; set; }

        protected CardRequest()
        {}

        public CardRequest(Individual individual, RequestType requestType, decimal creditLimit, decimal transAmountLimit, CardType cardType)
            : base(individual, requestType)
        {
            CardType = cardType;
            CreditLimit = creditLimit;
            TransactionAmountLimit = transAmountLimit;
        }
    }
}