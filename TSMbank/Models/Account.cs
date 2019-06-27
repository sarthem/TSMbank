using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public enum AccountStatus
    {
        Active,
        Inactive
    }

    public class Account
    {
        [Key]
        [StringLength(16)]
        public string AccountNumber { get; set; }
        
        public AccountStatus AccountStatus { get; set; }
        public decimal Balance { get; set; }
        public decimal WithdrawalLimit { get; set; }
        public string NickName { get; set; }//????
        public DateTime OpenedDate { get; set; }
        public DateTime StatusUpdatedDateTime { get; set; }

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
        public AccountType AccountType { get; set; }
        public byte AccountTypeId { get; set; }

        public ICollection<Transaction> CreditTransactions { get; set; }
        public ICollection<Transaction> DebitTransactions { get; set; }


        //Methods
        public static string CreateAccountRandomNumber()
        {
            int randomNumber = 0;
            var accountCreation = new List<int>();
            string accountNumber = "";
            int counter = 0;
            do
            {
                randomNumber = Bank.random.Next(1, 9999);
                bool check = accountCreation.Contains(randomNumber);
                if (!check)
                {
                    accountCreation.Add(randomNumber);
                    ++counter;
                }
            } while (counter < 5);
            //edw 8a proste8ei elenxos pros ola ta account
            foreach (int number in accountCreation)
            {
                accountNumber += number.ToString();               
                
            }
            return accountNumber;
        }

        public Account()
        {
            AccountNumber = CreateAccountRandomNumber();
            Balance = 0;
            OpenedDate = DateTime.Now;
            WithdrawalLimit = 1500;
            AccountStatus = AccountStatus.Inactive;
        }
    }
}