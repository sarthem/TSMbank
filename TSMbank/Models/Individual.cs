using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public enum IndividualStatus
    {
        Inactive,
        Active
    }


    
    public class Individual
    {
        
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }

        [Display(Name = "First name")]
        [StringLength(255)]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [StringLength(255)]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Father's name")]
        [StringLength(255)]
        [Required]
        public string FathersName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateOfBirth { get; set; }

        [Required]        
        [Display(Name = "Identity Card Number")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "This field must be 8 characters")]
        [RegularExpression("[a-zA-Z]{2}[0-9]{6}", ErrorMessage = "Identity Card Number must be 2 letters and 6 numbers")]
        public string IdentificationCardNo { get; set; }

        [Required]
        [Display(Name = "Social Security Number")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "This field must be 11 numbers")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Social Security Number must be numeric")]
        public string SSN { get; set; }

        [Required]
        [Display(Name = "Taxpayer Identification Number")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "This field must be 9 numbers")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Taxpayer Identification Number must be numeric")]
        public string VatNumber { get; set; }

        public DateTime CreatedDate { get; set; }
        public IndividualStatus Status { get; set; }

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
        public ICollection<BankAccount> BankAccounts { get; set; }

        //new code
        public ApplicationUser User { get; set; }
        

        public Individual()
        {
            CreatedDate = DateTime.Now;
            Status = IndividualStatus.Inactive;
        }

        public void Activate()
        {
            Status = IndividualStatus.Active;
        }

        public void Deactivate()
        {
            Status = IndividualStatus.Inactive;
        }
    }
}