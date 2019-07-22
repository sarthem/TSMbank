using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
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

        public Request()
        {

        }

        public Request(Individual individual, RequestType requestType)
        {
            Individual = individual;
            Status = RequestStatus.Pending;
            SubmissionDate = DateTime.Now;
            Type = requestType;
        }

        public virtual void Approve()
        {
            Status = RequestStatus.Approved;
            Individual.Activate();
            
        }
    }
}