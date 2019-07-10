using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.Dtos
{
    public class BankAccountTypeDto
    {
        public int Id { get; set; }
        public Description Description { get; set; }
        public decimal InterestRate { get; set; }
        public decimal PeriodicFee { get; set; }
        public string Summary { get; set; }
    }
}