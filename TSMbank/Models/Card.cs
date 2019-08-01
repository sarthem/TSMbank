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
        private const decimal DebitCardTransLimit = 1000.0m;
        private const string CreditCardBrand = "Visa";
        private const string DebitCardBrand = "MasterCard";


        public string Id { get; private set; }

        public string CardHolderName { get; private set; }
        public string Brand { get; private set; }
        public DateTime IssueDate { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public string CVV { get; private set; }
        public decimal TransactionAmountLimit { get; private set; }
        public decimal CreditLimit { get; private set; }
        public CardType Type { get; private set; }
        public CardStatus Status { get; private set; }

        [Required]
        [StringLength(16, MinimumLength = 16)]
        [Index(IsUnique = true)]
        public string Number { get; private set; }

        public BankAccount BankAccount { get; private set; }

        protected Card()
        { }

        private Card(BankAccount bankAccount, decimal transLimit, decimal creditLimit)
        {
            BankAccount = bankAccount ?? throw new NullReferenceException("bankAccount");
            CardHolderName = bankAccount.Individual.FullName;
            IssueDate = DateTime.Now;
            ExpiryDate = DateTime.Now.AddYears(4);
            CVV = Bank.random.Next(100, 1000).ToString();
            TransactionAmountLimit = transLimit;
            CreditLimit = creditLimit;
            Status = CardStatus.Active;
            Number = GenerateCardNumber();
        }

        public static Card CreditCard(BankAccount bankAccount, decimal transLimit, decimal creditLimit)
        {
            var creditCard = new Card(bankAccount, transLimit, creditLimit);
            creditCard.Type = CardType.CreditCard;
            creditCard.Brand = CreditCardBrand;
            bankAccount.Card = creditCard;
            return creditCard;
        }

        public static Card DebitCard(BankAccount bankAccount)
        {
            var debitCard = new Card(bankAccount, DebitCardTransLimit, 0);
            debitCard.Type = CardType.DebitCard;
            debitCard.Brand = DebitCardBrand;
            return debitCard;
        }

        private static string GenerateCardNumber()
        {
            int randomNumber = 0;
            var listOf4DigitNums = new List<int>();
            string cardNumber = "";
            int counter = 0;
            do
            {
                randomNumber = Bank.random.Next(1000, 9999);
                if (!listOf4DigitNums.Contains(randomNumber))
                {
                    listOf4DigitNums.Add(randomNumber);
                    counter++;
                }
            } while (counter < 4);

            foreach (int number in listOf4DigitNums)
                cardNumber += number.ToString();

            return cardNumber;
        }

        public string FormattedNumber()
        {
            var formattedNumber = "";
            for (var i = 0; i < Number.Length; i += 4)
            {
                formattedNumber += Number.Substring(i, Math.Min(4, Number.Length - i));
                if (Number.Length - i > 4)
                    formattedNumber += " ";
            }
            return formattedNumber;
        }

        public string Description()
        {
            if (Type == CardType.CreditCard)
                return BankAccount.BankAccountType.Summary;
            return Type.ToString();
        }

        public decimal CurrentBalance() // How much is the card charged
        {
            return CreditLimit - BankAccount.Balance;
        }
    }
}