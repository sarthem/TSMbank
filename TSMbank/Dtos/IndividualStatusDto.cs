using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.Dtos
{
    public class IndividualStatusDto
    {
        public string IndividualId { get; set; }
        public IndividualStatus Status { get; set; }
    }
}