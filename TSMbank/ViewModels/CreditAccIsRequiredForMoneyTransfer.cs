using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TSMbank.ViewModels
{
    public class CreditAccIsRequiredForMoneyTransfer : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var transferMoneyViewModel = (TransferMoneyViewModel) validationContext.ObjectInstance;

            if (String.IsNullOrEmpty(transferMoneyViewModel.CreditAccIban) && String.IsNullOrEmpty(transferMoneyViewModel.CreditAccNo))
                return new ValidationResult("You must specify the account the money will be transfered to.");
            return ValidationResult.Success;
        }
    }
}