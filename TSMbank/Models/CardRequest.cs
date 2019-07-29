using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TSMbank.Validations;

namespace TSMbank.Models
{
    public class CardRequest : Request
    {
        [Required]
        [Range(100, 5000, ErrorMessage = "Credit limit must be between 100 and 5000.")]
        public decimal CreditLimit { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        [CardTransLimit]
        public decimal TransactionAmountLimit { get; set; }

        public CardType CardType { get; set; }

        protected CardRequest()
        { }

        public CardRequest(Individual individual, RequestType requestType, decimal creditLimit, decimal transAmountLimit, CardType cardType)
            : base(individual, requestType)
        {
            CardType = cardType;
            CreditLimit = creditLimit;
            TransactionAmountLimit = transAmountLimit;
        }

        public override async Task Approve()
        {
            //var creditCardAcc = BankAccount.CreditCardAccount(Individual);
            //var creditCard = Card.CreditCard(creditCardAcc, TransactionAmountLimit, CreditLimit);
            var creditCardAcc = BankAccount.CreditCardAccount(Individual, TransactionAmountLimit, CreditLimit);
            Individual.BankAccounts.Add(creditCardAcc);
            Status = RequestStatus.Approved;
            var emailnfo = EmailInfo.CreditCardApproved(Individual);
            await emailnfo.Send();
        }

        public override async Task Reject()
        {
            Status = RequestStatus.Rejected;
            var emailnfo = EmailInfo.CreditCardRejected(Individual);
            await emailnfo.Send();
        }

        public override string TypeInfo()
        {
            return CardType.ToString();
        }
    }
}