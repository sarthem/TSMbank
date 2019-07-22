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
        private static readonly EmailAddress TsmBankEmail = new EmailAddress("PetitionDepartment@TSMbank.com", "TSM Bank");

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
                HtmlContent = $"<p>Dear {individual.FullName},</p><p>Your Petition for Activating your account has been APPROVED! Thank you for choosing TSM Bank."
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
                HtmlContent = $"<p>Dear {individual.FullName},</p><p>Your Bank Account has been activated! Thank you for choosing our Bank."
            };
            return emailInfo;
        }

        public static EmailInfo BankAccRejected(Individual individual)
        {
            var emailInfo = new EmailInfo()
            {
                From = new EmailAddress("PetitionDepartment@TSMbank.com", "TSM Bank"),
                Subject = "Reply On Bank Account Requets By TSM bank",
                To = new EmailAddress(individual.Email),
                PlainTextContent = "Your Petition is rejected",
                HtmlContent = "Your Petition for BankAccount creation has been REJECTED"
            };
        }

        public static async Task Send(EmailInfo emailInfo)
        {
            var apiKey = Environment.GetEnvironmentVariable("sendGridApiKey");
            var client = new SendGridClient(apiKey);
            var msg = MailHelper.CreateSingleEmail(From, email.To, email.Subject,
                email.PlainTextContent, email.HtmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}