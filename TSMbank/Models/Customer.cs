using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public enum CustomerStatus
    {
        Inactive,
        Active
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
        public string FathersName { get; set; }

        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string IdentificationCardNo { get; set; }
        public string SSN { get; set; }
        public string VatNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public CustomerStatus Status { get; set; }

        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }

        //Navigation Properties        
        public Address PrimaryAddress { get; set; }
        public int PrimaryAddressId { get; set; }

        public Address SecondaryAddress { get; set; }
        public int? SecondaryAddressId { get; set; }

        public ICollection<Phone> Phones { get; set; }
        public ICollection<BankAccount> Accounts { get; set; }

        //new code
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public Customer()
        {
            CreatedDate = DateTime.Now;
            Status = CustomerStatus.Inactive;
        }
    }
}