using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public enum AccountStatus
    {
        Active,
        Inactive
    }

    public class BankAccount
    {
        [Key]
        [StringLength(16,MinimumLength = 16)]
        public string AccountNumber { get; set; }

        public AccountStatus AccountStatus { get; set; }
        public decimal Balance { get; set; }
        public decimal WithdrawalLimit { get; set; }
        public string NickName { get; set; }
        public DateTime OpenedDate { get; set; }
        public DateTime? StatusUpdatedDateTime { get; set; }

        public string IndividualId { get; set; }
        public Individual Individual { get; set; }

        public string BBAN
        {
            get
            {
                return (Bank.BankCode + Bank.BranchCode + AccountNumber);
            }
        }

        public string IBAN
        {
            get
            {
                return (Bank.BankCountry + Bank.CheckDigit + BBAN);
            }
        }

        //Navigation Properties
        public BankAccountType BankAccountType { get; set; }

        [Display(Name = "Account Type")]
        public byte BankAccountTypeId { get; set; }

        public ICollection<Transaction> CreditTransactions { get; set; }

        public ICollection<Transaction> DebitTransactions { get; set; }

        //Methods
        public static string CreateRandomAccountNumber()
        {
            int randomNumber = 0;
            var accountCreation = new List<int>();
            string accountNumber = "";
            int counter = 0;
            do
            {
                randomNumber = Bank.random.Next(1000, 9999);
                bool check = accountCreation.Contains(randomNumber);
                if (!check)
                {
                    accountCreation.Add(randomNumber);
                    ++counter;
                }
            } while (counter < 4);
            //edw 8a proste8ei elenxos pros ola ta account
            foreach (int number in accountCreation)
            {
                accountNumber += number.ToString();

            }
            return accountNumber;
        }

        public BankAccount()
        {
            Balance = 0;
            WithdrawalLimit = 1500;
            AccountStatus = AccountStatus.Inactive;
        }
    }
}