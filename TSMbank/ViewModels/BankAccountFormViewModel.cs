using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TSMbank.Models;

namespace TSMbank.ViewModels
{
    public class BankAccountFormViewModel
    {
        public BankAccount BankAccount { get; set; }
        public List<BankAccountType> BankAccountTypes { get; set; }
        public string AccoutTypeDescription { get; set; }
        public string IndividualFullName { get; set; }

        public string Title
        {
            get { return BankAccount.AccountNumber == null ? "Create New Account" : "Edit Account"; }
        }
    }
}