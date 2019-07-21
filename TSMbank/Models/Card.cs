using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public class Card
    {
        //[Key]
        //[ForeignKey("BankAccount")]
        public string Id { get; set; }

        //[Key]
        //public string Number { get; set; }

        public string CardHolderName { get; set; }
        public string Brand { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string CVV { get; set; }
        public decimal TransactionAmountLimit { get; set; }
        public decimal CreditLimit { get; set; }
        public CardType Type { get; set; }
        public CardStatus Status { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 16)]
        [Index(IsUnique = true)]
        public string Number { get; set; }

        //[ForeignKey("BankAccount")]
        //[Index(IsUnique = true)]
        //public string BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

    }
}