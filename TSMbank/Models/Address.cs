using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public class Address
    {
        //Properties
        public int Id { get; set; }        
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string PostalCode { get; set; }
        public string Region { get; set; }

        public string FullAddress
        {
            get
            {
                return Street + " " +  StreetNumber + ", " + City + " " + "T.K." + PostalCode;
            }
        }

       
    }
}