using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TSMbank.Models
{
    public class BankAccRequest : Request
    {
        public byte BankAccTypeId { get; set; }
        public BankAccountType BankAccType { get; set; }
        //public string BankAccSummary { get; set; }

        protected BankAccRequest()
        { }

        public BankAccRequest(Individual individual, RequestType requestType, BankAccountType bankAccType)
            : base(individual, requestType)
        {
            BankAccType = bankAccType;
        }

        public BankAccRequest(Individual individual, RequestType requestType, byte bankAccTypeId)
            : base(individual, requestType)
        {
            BankAccTypeId = bankAccTypeId;
        }

        public async override Task Approve()
        {
            BankAccount bankAccount = null;
            switch (BankAccTypeId)
            {
                case BankAccountType.CheckingBasic:
                    bankAccount = BankAccount.CheckingBasic(Individual);
                    break;
                case BankAccountType.CheckingPremium:
                    bankAccount = BankAccount.CheckingPremium(Individual);
                    break;
                case BankAccountType.SavingsBasic:
                    bankAccount = BankAccount.SavingsBasic(Individual);
                    break;
                case BankAccountType.SavingsPremium:
                    bankAccount = BankAccount.SavingsPremium(Individual);
                    break;
                default:
                    break;
            }
            Individual.BankAccounts.Add(bankAccount);
            Status = RequestStatus.Approved;
            var emailnfo = EmailInfo.BankAccApproved(Individual);
            await emailnfo.Send();
        }

        public async override Task Reject()
        {
            Status = RequestStatus.Rejected;
            var emailnfo = EmailInfo.BankAccRejected(Individual);
            await emailnfo.Send();
        }

        public override string TypeInfo()
        {
            return BankAccType.Summary;
        }
    }
}