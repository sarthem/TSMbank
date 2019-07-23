using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TSMbank.Models
{
   public class BankAccRequest : Request
    {
        public int BankAccTypeId { get; set; }
        public BankAccountType BankAccType { get; set; }
        //public string BankAccSummary { get; set; }

        protected BankAccRequest()
        {}

        public BankAccRequest(Individual individual, RequestType requestType, BankAccountType bankAccType)
            : base(individual, requestType)
        {
            BankAccType = bankAccType;
        }

        public BankAccRequest(Individual individual, RequestType requestType, int bankAccTypeId)
            : base(individual, requestType)
        {
            BankAccTypeId = bankAccTypeId;
        }

        public async override Task Approve()
        {
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
    }
}