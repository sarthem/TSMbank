using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.Dtos
{
    public class TransactionViewModelDto
    {
        public DateTime ValueDate { get; set; }
        public string FinancialType { get; set; } // credit or debit
        public string RelatedAccInfo { get; set; } // Something that describes the related bank account eg. IBAN, or NickName if case of public services?
        public decimal Amount { get; set; }
        public string Category { get; set; }
    }
}