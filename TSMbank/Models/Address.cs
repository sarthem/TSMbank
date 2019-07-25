using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public class Address
    {
        //Properties
        public int Id { get; set; } 
        
        [Required]
        [StringLength(50, ErrorMessage = "This field must be maximum 50 characters")]
        public string Country { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "This field must be maximum 50 characters")]
        public string City { get; set; }

        [Required]
        [StringLength(255)]
        public string Street { get; set; }

        [Required]
        [StringLength(9, MinimumLength = 1, ErrorMessage = "This field must be maximum 9 characters")]
        public string StreetNumber { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "This field must be 5 numbers")]
        public string PostalCode { get; set; }

        
        [StringLength(255)]
        public string Region { get; set; }

        public string FullAddress
        {
            get
            {
                return Street + " " +  StreetNumber + ", " + City + " " + "T.K." + PostalCode;
            }
        }

        public Address(Address address)
        {
            City = address.City;
            Country = address.Country;
            PostalCode = address.PostalCode;
            Region = address.Region;
            Street = address.Street;
            StreetNumber = address.StreetNumber;   
        }

        public Address()
        {}


        public void Edit(Address address)
        {
            City = address.City;
            Country = address.Country;
            PostalCode = address.PostalCode;
            Region = address.Region;
            Street = address.Street;
            StreetNumber = address.StreetNumber;
        }

    }
}