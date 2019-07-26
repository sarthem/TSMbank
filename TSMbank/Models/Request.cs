using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TSMbank.Controllers;
using TSMbank.Validations;

namespace TSMbank.Models
{
    public class Request
    {
        public int Id { get; set; }

        public string IndividualId { get; set; }
        public Individual Individual { get; set; }

        public RequestStatus Status { get; set; }
        public DateTime SubmissionDate { get; set; }
        public RequestType Type { get; set; }

        public string Action
        {
            get
            {
                Expression<Func<RequestsController, ActionResult>> userAccReqDetails = (c => c.UserAccReqDetails(Id));
                Expression<Func<RequestsController, ActionResult>> bankAccReqDetails = (c => c.BankAccReqDetails(Id));
                Expression<Func<RequestsController, ActionResult>> cardReqDetails = (c => c.CardReqDetails(Id));
                Expression<Func<RequestsController, ActionResult>> action = null;
                switch (Type)
                {
                    case RequestType.UserAccActivation:
                        action = userAccReqDetails;
                        break;
                    case RequestType.BankAccActivation:
                        action = bankAccReqDetails;
                        break;
                    case RequestType.CardActivation:
                        action = cardReqDetails;
                        break;
                    case RequestType.LoanActivation:
                        break;
                    default:
                        break;
                }
                return (action.Body as MethodCallExpression).Method.Name;
            }
        }

        protected Request()
        { }

        public Request(Individual individual, RequestType requestType)
        {
            Individual = individual;
            Status = RequestStatus.Pending;
            SubmissionDate = DateTime.Now;
            Type = requestType;
        }

        public virtual async Task Approve()
        {
            Status = RequestStatus.Approved;
            Individual.Activate();
            var emailInfo = EmailInfo.AccApproved(Individual);
            await emailInfo.Send();
        }

        public virtual async Task Reject()
        {
            Status = RequestStatus.Rejected;
            Individual.Deactivate();
            var emailInfo = EmailInfo.AccRejected(Individual);
            await emailInfo.Send();
        }

        public virtual string TypeInfo()
        {
            return "Customer activation";
        }
    }
}