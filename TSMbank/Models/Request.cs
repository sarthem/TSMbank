using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{ 
    public enum RequestName
    {
        Account_Activation,
        BankAccount_Activation,
        CreditCard_Approval
    }
    public enum RequestStatus
    {
        Pending,
        Approved,
        Rejected
    }
    public class Request
    {
        public string Id { get; set; }        
        public RequestName Name { get; set; }
        public RequestStatus Status { get; set; }
        public string IndividualId { get; set; }
        public Individual Individual { get; set; }


        //i edw na min mpoun aytakai na ginun sub class kai ayti edw na ginei class

        public string BankAccountNo { get; set; }
        public BankAccount BankAccount { get; set; }
        // ta tis credit card


    }
}