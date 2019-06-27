using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public enum Activation
    {
        IsActive,
        NotActive
    }


    
    public class Customer
    {
        //Properties
        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(255)]
        [Required]
        public string LastName { get; set; }

        [StringLength(255)]
        [Required]
        public string FatherName { get; set; }

        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string IdentificationCardNo { get; set; }
        public string SSN { get; set; }
        public string VatNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public Activation IsActive { get; set; }

        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }
        //Navigation Properties        
        
        public Address Address { get; set; }
        public int AddressId { get; set; }

        public Phone Phone { get; set; }
        public int PhoneId { get; set; }



        public ICollection<Phone> PhoneNumbers { get; set; } 
        public ICollection<Address>Addresses { get; set; }
        public ICollection<Account> Accounts { get; set; }


      


        //contractor
        public Customer()
        {
            IsActive = Activation.NotActive;
            CreatedDate = DateTime.Now;
        }
    }
}