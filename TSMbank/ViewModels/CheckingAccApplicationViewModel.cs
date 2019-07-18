using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.ViewModels
{
    public class CheckingAccApplicationViewModel
    {
        public IndividualStatus IndividualStatus { get; set; }
        public IEnumerable<BankAccountType> CheckingAccountTypes { get; set; }
        public int AccTypeId { get; set; }
    }
}