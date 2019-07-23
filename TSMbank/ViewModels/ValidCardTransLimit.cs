using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TSMbank.ViewModels
{
    public class ValidCardTransLimit : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var cardRequest = (CardReqViewModel) validationContext.ObjectInstance;

            if (cardRequest.TransactionAmountLimit <= cardRequest.CreditLimit)
                return ValidationResult.Success;
            else
                return new ValidationResult("Transacion amount limit must be lower or equal to Credit Limit.");
        }
    }
}