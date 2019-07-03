using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TSMbank.Models;

namespace TSMbank.ViewModels
{
    public class AccountFormViewModel
    {
        public Account Account { get; set; }
        public List<AccountType> AccountTypes { get; set; }
        public string CustomerFullName { get; set; }

        public string Title
        {
            get { return Account.AccountNumber == null ? "Create New Account" : "Edit Account"; }
        }
    }
}