using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.Dtos
{
    public class BankAccountDto
    {
        [StringLength(16)]
        public string AccountNumber { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public decimal Balance { get; set; }
        public decimal WithdrawalLimit { get; set; }
        public string NickName { get; set; }
        public DateTime OpenedDate { get; set; }
        public DateTime? StatusUpdatedDateTime { get; set; }

        public string IndividualId { get; set; }
        public byte AccountTypeId { get; set; }
        
    }
}