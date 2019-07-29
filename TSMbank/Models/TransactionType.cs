﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public class TransactionType
    {
        public byte Id { get; set; }
        public TransactionCategory Category { get; set; }
        public decimal Fee { get; set; }
        public string Description { get; set; }
    }
}