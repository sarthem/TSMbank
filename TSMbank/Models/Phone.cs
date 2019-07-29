using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TSMbank.Models
{
    public class Phone
    {
        public int Id { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "This field must be 4 numbers (0030)")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone Number must be numeric (0030)")]
        public string CountryCode { get; set; }

        [Required]        
        [Display(Name = "Contact Phone Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "This field must be 10 numbers")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone Number must be numeric")]
        public string PhoneNumber { get; set; }

        public PhoneType PhoneType { get; set; }

        public string IndividualId { get; set; }
        public Individual Individual { get; set; }

        [Display(Name = "Calling Number")]
        public string CallingNumber
        {
            get
            {
                return (CountryCode + " " + PhoneNumber);
            }
        }

        public void Edit(Phone phone)
        {
            CountryCode = phone.CountryCode;
            PhoneNumber = phone.PhoneNumber;
            PhoneType = phone.PhoneType;
        }



    }
}