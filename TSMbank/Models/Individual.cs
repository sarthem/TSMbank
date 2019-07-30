using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TSMbank.ViewModels;

namespace TSMbank.Models
{
    
    public class Individual
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; private set; }

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

        [Required]
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
        
        public ApplicationUser User { get; set; }

       
        //public Individual()//extra
        //{}
        //contructors
        public  Individual()//for new private
        {
            CreatedDate = DateTime.Now;
            Status = IndividualStatus.Inactive;
            Phones = new Collection<Phone>();
            BankAccounts = new Collection<BankAccount>();
        }

        public Individual(string fathersName, DateTime? dateOfBirth, string firstName, string identificationCardNo
            , string lastName, string sSN, string vatNumber, string id, string email, Collection<Phone> phones
            , Address address)
        {
            FathersName = fathersName;            
            DateOfBirth = dateOfBirth;            
            FirstName = firstName;            
            IdentificationCardNo = identificationCardNo;
            LastName = lastName;
            SSN = sSN;
            VatNumber = vatNumber;
            Id = id;
            CreatedDate = DateTime.Now;
            Status = IndividualStatus.Inactive;
            Email = email;
            Phones = phones;
            BankAccounts = new Collection<BankAccount>();
            PrimaryAddress = address;
           
        }


        public static Individual NewForView()
        {
            var individual = new Individual();

            return individual;

        }

        

        public static Individual New(IndividualFormViewModel IVM, ApplicationUser user, Collection<Phone> phones)
        {
            var individual = new Individual(IVM.Individual.FathersName, IVM.Individual.DateOfBirth,
                IVM.Individual.FirstName, IVM.Individual.IdentificationCardNo, IVM.Individual.LastName,
                IVM.Individual.SSN, IVM.Individual.VatNumber, user.Id, user.Email, phones, IVM.PrimaryAddress);

            return individual;
        }

        public void Activate()
        {
            Status = IndividualStatus.Active;
        }

        public void Deactivate()
        {
            Status = IndividualStatus.Inactive;
        }
        public void Edit(Individual individual)
        {
            DateOfBirth = individual.DateOfBirth;
            Email = individual.Email;
            FathersName = individual.FathersName;
            FirstName = individual.FirstName;
            IdentificationCardNo = individual.IdentificationCardNo;
            LastName = individual.LastName;
            SSN = individual.SSN;
            VatNumber = individual.VatNumber;
        }

        public void SetEmail(ApplicationUser User)
        {
            Email = User.Email;
        }
    }
}