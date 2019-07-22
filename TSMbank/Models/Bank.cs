using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public static class Bank
    {
        public static readonly string BankCountry = "GR";
        public static readonly string CheckDigit = "16";
        public static readonly string BankCode = "893";
        public static readonly string BranchCode = "101";
        public static Random random = new Random();
    }
}