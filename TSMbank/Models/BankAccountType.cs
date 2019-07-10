using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public enum Description
    {
        Checking,
        Savings,
        Term
    }

    public class BankAccountType
    {

        //Properties
        public int Id { get; set; }
        public Description Description { get; set; }
        public decimal InterestRate { get; set; }
        public decimal PeriodicFee { get; set; }
        public string Summary { get; set; }


        //Navigation Properties
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
    }
}