using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.Validations
{
    public class CardTransLimit : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var creditCardRequest = (CardRequest) validationContext.ObjectInstance;

            if (creditCardRequest.TransactionAmountLimit <= creditCardRequest.CreditLimit)
                return ValidationResult.Success;
            else
                return new ValidationResult("Transacion amount limit must be lower or equal to Credit Limit.");
        }
    }
}