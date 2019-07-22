using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
   public class BankAccRequest : Request
    {
        public int BankAccTypeId { get; set; }
        public string BankAccSummury { get; set; }
        
    }
}