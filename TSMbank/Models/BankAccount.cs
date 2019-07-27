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
        private const decimal CheckingWithDrawalLimit = 1500.0m;
        private const decimal TsmVisaClassicWithDrawalLimit = 0m;
        private const decimal SavingsWithdrawalLimit = Decimal.MaxValue;

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

        public BankAccountType BankAccountType { get; set; }

        [Display(Name = "Account Type")]
        public byte BankAccountTypeId { get; set; }

        public ICollection<Transaction> CreditTransactions { get; set; }
        public ICollection<Transaction> DebitTransactions { get; set; }
        public Card Card { get; set; }

        public BankAccount()
        {
            Balance = 0;
            WithdrawalLimit = 1500;
            AccountStatus = AccountStatus.Inactive;
        }

        private BankAccount(Individual individual, decimal withdrawlLimit, byte accountTypeId)
        {
            AccountNumber = CreateRandomAccountNumber();
            AccountStatus = AccountStatus.Active;
            Balance = 0;
            OpenedDate = DateTime.Now;
            StatusUpdatedDateTime = DateTime.Now;
            Individual = individual;
            WithdrawalLimit = withdrawlLimit;
            BankAccountTypeId = accountTypeId;
        }

        private BankAccount(string individualId, decimal withdrawlLimit, byte accountTypeId)
        {
            AccountNumber = CreateRandomAccountNumber();
            AccountStatus = AccountStatus.Active;
            Balance = 0;
            OpenedDate = DateTime.Now;
            StatusUpdatedDateTime = DateTime.Now;
            IndividualId = individualId;
            WithdrawalLimit = withdrawlLimit;
            BankAccountTypeId = accountTypeId;
        }

        public static BankAccount CreditCardAccount(Individual individual)
        {
            return new BankAccount(individual, TsmVisaClassicWithDrawalLimit, BankAccountType.TSMVisaClassic);
        }

        public static BankAccount CreditCardAccount(string individualId)
        {
            return new BankAccount(individualId, TsmVisaClassicWithDrawalLimit, BankAccountType.TSMVisaClassic);
        }

        public static BankAccount CheckingBasic(Individual individual)
        {
            return new BankAccount(individual, CheckingWithDrawalLimit, BankAccountType.CheckingBasic);
        }

        public static BankAccount CheckingPremium(Individual individual)
        {
            return new BankAccount(individual, CheckingWithDrawalLimit, BankAccountType.CheckingPremium);
        }

        public static BankAccount SavingsBasic(Individual individual)
        {
            return new BankAccount(individual, SavingsWithdrawalLimit, BankAccountType.SavingsBasic);
        }

        public static BankAccount SavingsPremium(Individual individual)
        {
            return new BankAccount(individual, SavingsWithdrawalLimit, BankAccountType.SavingsPremium);
        }

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

            foreach (int number in accountCreation)
            {
                accountNumber += number.ToString();

            }
            return accountNumber;
        }
    }
}