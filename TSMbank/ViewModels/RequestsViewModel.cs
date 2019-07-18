using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.ViewModels
{
    public class RequestsViewModel
    {
        public List<Request> Requests { get; set; }
        public RequestStatus Status { get; set; }
    }
}