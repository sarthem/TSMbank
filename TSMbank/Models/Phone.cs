using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TSMbank.Models
{
    public enum PhoneType
    {
        Mobile,
        Home,
        Work
    };


    public class Phone
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
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





    }
}