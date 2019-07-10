using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Util;
using TSMbank.Models;

namespace TSMbank.ViewModels
{
    public class TransactionsDetailsViewModel
    {
        public IEnumerable<Transaction> Transactions { get; set; }
        public string AccountNumber { get; set; }

    }
}