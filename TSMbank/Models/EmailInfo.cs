using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace TSMbank.Models
{
    public class EmailInfo
    {
        private static readonly EmailAddress TsmBankEmail = new EmailAddress("petition.department@tsmbank.com", "TSM Bank");

        public int Id { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
        public string PlainTextContent { get; set; }
        public Individual Individual { get; set; }

        public EmailAddress To
        {
            get
            {
                return new EmailAddress(Individual.Email);
            }
        }

        protected EmailInfo() { }

        public static EmailInfo AccApproved(Individual individual)
        {
            var emailInfo = new EmailInfo()
            {
                Individual = individual,
                Subject = "TSM Bank - Account Activation",
                PlainTextContent = "Your Petition has been approved.",
                HtmlContent = $"<p>Dear {individual.FullName},</p><p>Your account has been activated! Thank you for choosing TSM Bank.</p>"
            };
            return emailInfo;
        }

        public static EmailInfo AccRejected(Individual individual)
        {
            var emailInfo = new EmailInfo()
            {
                Individual = individual,
                Subject = "TSM Bank - Account Activation",
                PlainTextContent = "Your Petition has been rejected.",
                HtmlContent = $"<p>Dear {individual.FullName},</p><p>We are sorry to inform you that your account activation petition has been rejected. We apologize for any inconvenience.</p>"
            };
            return emailInfo;
        }

        public static EmailInfo BankAccApproved(Individual individual)
        {
            var emailInfo = new EmailInfo()
            {
                Individual = individual,
                Subject = "TSM Bank - Bank Account Activation",
                PlainTextContent = "Your Petition has been approved.",
                HtmlContent = $"<p>Dear {individual.FullName},</p><p>Your Bank Account has been activated! Thank you for choosing TSM Bank.</p>"
            };
            return emailInfo;
        }

        public static EmailInfo BankAccRejected(Individual individual)
        {
            var emailInfo = new EmailInfo()
            {
                Individual = individual,
                Subject = "TSM Bank - Bank Account Activation",
                PlainTextContent = "Your Petition has been rejected.",
                HtmlContent = $"<p>Dear {individual.FullName},</p><p>Your petition for Bank Account activation has been rejected. We apologize for any inconvenience.</p>"
            };
            return emailInfo;
        }

        public static EmailInfo CreditCardApproved(Individual individual)
        {
            var emailInfo = new EmailInfo()
            {
                Individual = individual,
                Subject = "TSM Bank - Credit Card Activation",
                PlainTextContent = "Your Petition has been approved.",
                HtmlContent = $"<p>Dear {individual.FullName},</p><p>Your new TSM Visa Classic has been activated! Thank you for choosing TSM Bank.</p>"
            };
            return emailInfo;
        }

        public static EmailInfo CreditCardRejected(Individual individual)
        {
            var emailInfo = new EmailInfo()
            {
                Individual = individual,
                Subject = "TSM Bank - Credit Card Activation",
                PlainTextContent = "Your Petition has been rejected.",
                HtmlContent = $"<p>Dear {individual.FullName},</p><p>We are sorry to inform you that your credit card activation petition has been rejected. We apologize for any inconvenience.</p>"
            };
            return emailInfo;
        }

        public async Task Send()
        {
            var apiKey = Environment.GetEnvironmentVariable("sendGridApiKey", EnvironmentVariableTarget.User);
            var client = new SendGridClient(apiKey);
            var msg = MailHelper.CreateSingleEmail(TsmBankEmail, new EmailAddress(Individual.Email), Subject, PlainTextContent, HtmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}