using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public class Min18Years : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = (Individual)validationContext.ObjectInstance;
            
            if (user.DateOfBirth == null)
            {
                return new ValidationResult("Birthday is required");
            };

            var age = DateTime.Today.Year - user.DateOfBirth.Value.Year;

            return (age >= 18) ? ValidationResult.Success
                : new ValidationResult("Viewer must be at least 18 years old");
        }
    }
}