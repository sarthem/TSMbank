using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public enum RequestStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public enum RequestType
    {
        UserAccActivation,
        BankAccActivation,
        CreditCardActivation,
        LoanActivation
    }

    public class Request
    {
        public int Id { get; set; }

        public string IndividualId { get; set; }
        public ApplicationUser Individual { get; set; }

        public RequestStatus Status { get; set; }
        public DateTime SubmissionDate { get; set; }
        public RequestType Type { get; set; }

    }

    public class BankAccRequest : Request
    {
        public string BankAccNumber { get; set; }
        public BankAccount BankAccount { get; set; }
    }

    public class CreditCardRequest : Request
    {

    }
}