using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Payment,
        Bankfee,
        Cancellation
    }

    public enum TransactionApprovedReview
    {
        Approve,
        Reject
    }

    public class Transaction
    {       
        public int TransactionId { get; set; }
        public TransactionType Type { get; set; }
        public DateTime ValueDateTime { get; set; } // timestamp

        public string CreditAccountNo { get; set; }
        public string CreditIBAN { get; set; }
        public decimal CreditAccountBalance { get; set; }
        public string CreditAccountCurrency { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal CreditAccountBalanceAfterTransaction { get; set; }

        public string DebitAccountNo { get; set; }
        public string DebitIBAN { get; set; }
        public decimal DebitAccountBalance { get; set; }
        public decimal DebitAccountCurrency { get; set; }
        public int DebitAmount { get; set; }
        public int DebitAccountBalanceAfterTransaction { get; set; }

        public int BankFee { get; set; } 
        public bool ApprovedFromBankManager { get; set; }
        public bool PendingForApproval { get; set; }
        public TransactionApprovedReview TransactionApprovedReview { get; set; }
        public bool IsCompleted { get; set; }
        public int? CancelledTransactionId { get; set; } //reference old transaction

        //Navigation properties

        public Account CreditAccount { get; set; }
        public Account DebitAccount { get; set; }



        [ForeignKey("CancelledTransactionId")]
        public Transaction CancelledTransaction { get; set; }
    }
}