using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.Dtos
{
    public class TransactionInfoDto
    {
        public string DebitAccNo { get; set; }
        public string CreditAccNo { get; set; }
        public decimal Amount { get; set; }
        public TransactionCategory Category { get; set; }
    }
}