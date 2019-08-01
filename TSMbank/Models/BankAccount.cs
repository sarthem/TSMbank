using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public class BankAccount
    {
        private const decimal CheckingWithDrawalLimit = 1500.0m;
        private const decimal TsmVisaClassicWithDrawalLimit = 0m;
        private const decimal SavingsWithdrawalLimit = 99999999999.99m;
        private const decimal InitialBalance = 5000.0m;
        private const string CheckinBasicInitialNickName = "Checking Basic";
        private const string CheckinPremiumInitialNickName = "Checking Premium";
        private const string SavingsBasicInitialNickName = "Savings Basic";
        private const string SavingsPremiumInitialNickName = "Savings Premium";
        private const string CreditCardAccInitialNickName = "TSM Visa Classic";
        

        [Key]
        [StringLength(16, MinimumLength = 16)]
        public string AccountNumber { get; set; }

        public AccountStatus AccountStatus { get; set; }
        public decimal Balance { get; private set; }
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

        public ICollection<Transaction> CreditTransactions { get; private set; }
        public ICollection<Transaction> DebitTransactions { get; private set; }
        public Card Card { get; set; }

        public BankAccount()
        {
            Balance = InitialBalance;
            WithdrawalLimit = 1500;
            AccountStatus = AccountStatus.Inactive;
        }

        private BankAccount(Individual individual, decimal withdrawlLimit, byte accountTypeId)
        {
            AccountNumber = CreateRandomAccountNumber();
            AccountStatus = AccountStatus.Active;
            Balance = InitialBalance;
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
            Balance = InitialBalance;
            OpenedDate = DateTime.Now;
            StatusUpdatedDateTime = DateTime.Now;
            IndividualId = individualId;
            WithdrawalLimit = withdrawlLimit;
            BankAccountTypeId = accountTypeId;
        }

        public static BankAccount CreditCardAccount(Individual individual, decimal transLimit, decimal creditLimit)
        {
            var creditCardAcc = new BankAccount(individual, TsmVisaClassicWithDrawalLimit, BankAccountType.TSMVisaClassic);
            var creditCard = Card.CreditCard(creditCardAcc, transLimit, creditLimit);
            creditCardAcc.Balance = creditLimit;
            creditCardAcc.NickName = CreditCardAccInitialNickName;
            return creditCardAcc;
        }

        public static BankAccount CreditCardAccount(string individualId, decimal transLimit, decimal creditLimit)
        {
            var creditCardAcc = new BankAccount(individualId, TsmVisaClassicWithDrawalLimit, BankAccountType.TSMVisaClassic);
            creditCardAcc.NickName = CreditCardAccInitialNickName;
            var creditCard = Card.CreditCard(creditCardAcc, transLimit, creditLimit);
            creditCardAcc.Balance = creditLimit;
            return creditCardAcc;
        }

        public static BankAccount CheckingBasic(Individual individual)
        {
            var checkingBasic = new BankAccount(individual, CheckingWithDrawalLimit, BankAccountType.CheckingBasic);
            checkingBasic.NickName = CheckinBasicInitialNickName;
            checkingBasic.Card = Card.DebitCard(checkingBasic);
            return checkingBasic;
        }

        public static BankAccount CheckingPremium(Individual individual)
        {
            var checkingPremium = new BankAccount(individual, CheckingWithDrawalLimit, BankAccountType.CheckingPremium);
            checkingPremium.NickName = CheckinPremiumInitialNickName;
            checkingPremium.Card = Card.DebitCard(checkingPremium);
            return checkingPremium;
        }

        public static BankAccount SavingsBasic(Individual individual)
        {
            var savingsBasic = new BankAccount(individual, SavingsWithdrawalLimit, BankAccountType.SavingsBasic);
            savingsBasic.NickName = SavingsBasicInitialNickName;
            return savingsBasic;
        }

        public static BankAccount SavingsPremium(Individual individual)
        {
            var savingsPremium = new BankAccount(individual, SavingsWithdrawalLimit, BankAccountType.SavingsPremium);
            savingsPremium.NickName = SavingsPremiumInitialNickName;
            return savingsPremium;
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

        public string ProductIdentifier()
        {
            if (BankAccountType.Description == Description.CreditCard && Card != null)
                return Card.FormattedNumber();
            return IBAN;
        }

        public bool InitiateTransaction(BankAccount creditAcc, decimal amount, TransactionType transType,
            BankAccount tsmBankAcc, out List<Transaction> transactions)
        {
            Transaction transaction = null;
            transactions = new List<Transaction>();
            if (amount <= 0)
                return false;

            if (transType.Category == TransactionCategory.Payment || transType.Category == TransactionCategory.MoneyTransfer)
            {
                if (Balance - amount - transType.Fee < 0)
                    return false;
                var newBalance = Balance - amount;
                var newCreditAccBalance = creditAcc.Balance + amount;
                transaction = new Transaction(transType, this, creditAcc, amount, Balance, newBalance, amount, creditAcc.Balance, newCreditAccBalance);
                Balance = newBalance;
                creditAcc.Balance = newCreditAccBalance;
                transactions.Add(transaction);
                //DebitTransactions.Add(transaction);
                if (transType.Fee > 0)
                {
                    newBalance = Balance - transType.Fee;
                    var newTsmBankAccBalance = tsmBankAcc.Balance + transType.Fee;
                    var commissionTrans = Transaction.BankCommission(this, tsmBankAcc, transType.Fee, Balance,
                        newBalance, transType.Fee, tsmBankAcc.Balance, newTsmBankAccBalance);
                    Balance = newBalance;
                    tsmBankAcc.Balance = newTsmBankAccBalance;
                    transactions.Add(commissionTrans);
                    //DebitTransactions.Add(commissionTrans);
                }
                return true;
            }
            return false;
        }
    }
}