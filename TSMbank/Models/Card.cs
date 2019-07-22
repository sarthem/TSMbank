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
        public BankAccount BankAccount { get; set; }

        protected Card()
        {}

        public static Card CreditCard(BankAccount bankAccount, decimal transLimit, decimal creditLimit)
        {
            if (bankAccount == null)
                throw new NullReferenceException("bankAccount");

            var creditCard = new Card()
            {
                BankAccount = bankAccount,
                CardHolderName = bankAccount.Individual.FullName,
                Brand = "Visa",
                IssueDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(4),
                CVV = Bank.random.Next(100, 1000).ToString(),
                TransactionAmountLimit = transLimit,
                CreditLimit = creditLimit,
                Type = CardType.CreditCard,
                Status = CardStatus.Active,
                Number = GenerateCardNumber()
            };
            return creditCard;
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
    }
}