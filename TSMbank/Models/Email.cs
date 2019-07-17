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
    public class Email
    {
        public int Id { get; set; }
        public EmailAddress From { get; set; }
        public EmailAddress To { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
        public string PlainTextContent { get; set; }


        public static async Task SendMail(Email email)
        {
            var apiKey = Environment.GetEnvironmentVariable("sendGridApiKey2");
            var client = new SendGridClient(apiKey);
            var msg = MailHelper.CreateSingleEmail(email.From, email.To, email.Subject, 
                email.PlainTextContent, email.HtmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}