using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{


    public class BankAccountType
    {
        public const byte CheckingBasic = 11;
        public const byte CheckingPremium = 12;
        public const byte SavingsBasic = 21;
        public const byte SavingsPremium = 22;
        public const byte TSMVisaClassic = 31;
        public const byte TermBasic = 41;
        public const byte PublicServices = 51;


        public byte Id { get; set; }
        public Description Description { get; set; }
        public decimal InterestRate { get; set; }
        public decimal PeriodicFee { get; set; }
        public string Summary { get; set; }


        //Navigation Properties
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
    }
}