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
        public static readonly string UserId = "3g81b2cc-2659-49cc-9537-1b25d2273123";
        public static readonly string AccNumber = "1111222233334444";
        public static readonly string IBAN = "GR168931011111222233334444";
        public static readonly string Currency = "Eur";
    }
}