using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.Dtos
{
    public class CustomerStatusDto
    {
        public int CustomerId { get; set; }
        public CustomerStatus Status { get; set; }
    }
}