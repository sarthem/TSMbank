using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    

    public class Transaction
    {       
        public int TransactionId { get; private set; }

        [ForeignKey("Type")]
        public byte TypeId { get; private set; }
        public TransactionType Type { get; private set; }

        public DateTime ValueDateTime { get; private set; } // timestamp

        public string DebitAccountNo { get; private set; }
        public string DebitIBAN { get; private set; }
        public decimal DebitAccountBalance { get; private set; }
        public string DebitAccountCurrency { get; private set; }

        //[Range(1, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public decimal DebitAmount { get; private set; }

        public decimal DebitAccountBalanceAfterTransaction { get; private set; }

        public string CreditAccountNo { get; private set; }
        public string CreditIBAN { get; private set; }
        public decimal CreditAccountBalance { get; private set; }
        public string CreditAccountCurrency { get; private set; }
        public decimal CreditAmount { get; private set; }
        public decimal CreditAccountBalanceAfterTransaction { get; private set; }

        public bool IsCompleted { get; private set; }
        public int? CancelledTransactionId { get; private set; } //reference old transaction

        //Navigation properties
        public BankAccount DebitAccount { get; private set; }
        public BankAccount CreditAccount { get; private set; }
       
        [ForeignKey("CancelledTransactionId")]
        public Transaction CancelledTransaction { get; private set; }

        protected Transaction()
        {}

        public Transaction(TransactionType type, BankAccount debitAcc, BankAccount creditAcc, decimal debitAmount, decimal debitAccBalance,
            decimal newDebitAccBalance, decimal creditAmount, decimal creditAccBalance, decimal newCreditAccBalance)
        {
            if (debitAcc == null || creditAcc == null)
                throw new ArgumentNullException("creditAcc/debitAcc");
            Type = type;
            DebitAccount = debitAcc;
            CreditAccount = creditAcc;
            DebitAmount = debitAmount;
            DebitAccountBalance = debitAccBalance;
            DebitAccountBalanceAfterTransaction = newDebitAccBalance;
            CreditAmount = creditAmount;
            CreditAccountBalance = creditAccBalance;
            CreditAccountBalanceAfterTransaction = newCreditAccBalance;
            ValueDateTime = DateTime.Now;
            DebitIBAN = debitAcc.IBAN;
            DebitAccountCurrency = Bank.Currency;
            CreditIBAN = creditAcc.IBAN;
            CreditAccountCurrency = Bank.Currency;
            IsCompleted = true;
            CancelledTransaction = null;
        }

        private Transaction(byte transTypeId, BankAccount debitAcc, BankAccount creditAcc, decimal debitAmount, decimal debitAccBalance,
            decimal newDebitAccBalance, decimal creditAmount, decimal creditAccBalance, decimal newCreditAccBalance)
        {
            if (debitAcc == null || creditAcc == null)
                throw new ArgumentNullException("creditAcc/debitAcc");
            TypeId = transTypeId;
            DebitAccount = debitAcc;
            CreditAccount = creditAcc;
            DebitAmount = debitAmount;
            DebitAccountBalance = debitAccBalance;
            DebitAccountBalanceAfterTransaction = newDebitAccBalance;
            CreditAmount = creditAmount;
            CreditAccountBalance = creditAccBalance;
            CreditAccountBalanceAfterTransaction = newCreditAccBalance;
            ValueDateTime = DateTime.Now;
            DebitIBAN = debitAcc.IBAN;
            DebitAccountCurrency = Bank.Currency;
            CreditIBAN = creditAcc.IBAN;
            CreditAccountCurrency = Bank.Currency;
            IsCompleted = true;
            CancelledTransaction = null;
        }

        //public static Transaction Payment(BankAccount debitAcc, BankAccount creditAcc, decimal debitAmount, decimal debitAccBalance,
        //    decimal newDebitAccBalance, decimal creditAmount, decimal creditAccBalance, decimal newCreditAccBalance)
        //{
        //    return new Transaction(TransactionType.Payment, debitAcc, creditAcc, debitAmount, debitAccBalance,
        //        newDebitAccBalance, creditAmount, creditAccBalance, newCreditAccBalance);
        //}

        //public static Transaction MoneyTransfer(BankAccount debitAcc, BankAccount creditAcc, decimal debitAmount, decimal debitAccBalance,
        //    decimal newDebitAccBalance, decimal creditAmount, decimal creditAccBalance, decimal newCreditAccBalance)
        //{
        //    return new Transaction(TransactionType.MoneyTransfer, debitAcc, creditAcc,debitAmount, debitAccBalance,
        //        newDebitAccBalance, creditAmount, creditAccBalance, newCreditAccBalance);
        //}

        public static Transaction BankCommission(BankAccount debitAcc, BankAccount tsmBankAcc, decimal debitAmount, decimal debitAccBalance,
            decimal newDebitAccBalance, decimal creditAmount, decimal creditAccBalance, decimal newCreditAccBalance)
        {
            return new Transaction(TransactionType.BankCommission, debitAcc, tsmBankAcc, debitAmount, debitAccBalance,
                newDebitAccBalance, creditAmount, creditAccBalance, newCreditAccBalance);
        }

        public string RelatedAccInfo(BankAccount bankAcc)
        {
            if (bankAcc.AccountNumber != DebitAccountNo && bankAcc.AccountNumber != CreditAccountNo)
                throw new InvalidOperationException("Invalid Bank Account");

            var relatedAcc = bankAcc.AccountNumber == DebitAccountNo ? CreditAccount : DebitAccount;
            var relatedAccNickName = relatedAcc.NickName ?? ""; 
            if (relatedAcc.BankAccountType.Description == Description.PublicServices || relatedAcc.AccountNumber == Bank.AccNumber)
                return relatedAccNickName;
            return relatedAcc.IBAN;
            //return relatedAccNickName + " (" + relatedAcc.IBAN + ")";
        }

        public string GetFinancialType(BankAccount bankAcc)
        {
            if (bankAcc.AccountNumber != DebitAccountNo && bankAcc.AccountNumber != CreditAccountNo)
                throw new InvalidOperationException("Invalid Bank Account");
            return bankAcc.AccountNumber == DebitAccountNo ? "Debit" : "Credit";
        }
    }
}