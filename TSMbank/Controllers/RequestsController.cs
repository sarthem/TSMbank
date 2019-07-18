using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using TSMbank.Models;
using TSMbank.ViewModels;

namespace TSMbank.Controllers
{
    public class RequestsController : Controller
    {

        private ApplicationDbContext context;

        public RequestsController()
        {
            context = new ApplicationDbContext();
        }

        // GET: Requests
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult GetRequests(RequestStatus status)
        {
            
            var requests = new List<Request>();
            if (status == RequestStatus.Pending)
            {
                requests = context.Requests
                .Include(r => r.Individual)
                .Include(r => r.Individual.Individual)
                .Where(r => r.Status == RequestStatus.Pending)
                .ToList();
            }else if (status == RequestStatus.Approved)
            {
                 requests = context.Requests
                .Include(r => r.Individual)
                .Include(r => r.Individual.Individual)
                .Where(r => r.Status == RequestStatus.Approved)
                .ToList();
            }
            else if (status == RequestStatus.Rejected)
            {
                requests = context.Requests
               .Include(r => r.Individual)
               .Include(r => r.Individual.Individual)
               .Where(r => r.Status == RequestStatus.Rejected)
               .ToList();
            }
            var viewModel = new RequestsViewModel
            {
                Requests = requests,
                Status = status
            };

            return View(viewModel);
        }


        public ActionResult RequestAnswer(int Id, RequestStatus requestStatus, RequestType requestType )
        {
            
            switch (requestType)
            {
                case RequestType.UserAccActivation:
                    var request1 = context.Requests
                                    .Include(r => r.Individual)
                                    .SingleOrDefault(r => r.Id == Id);
                    var individual = context.Individuals
                        .SingleOrDefault(c => c.Id == request1.IndividualId);
                    if (requestStatus == RequestStatus.Approved)
                    {
                        request1.Status = requestStatus;
                        request1.Individual.Individual.Status = IndividualStatus.Active;
                        var emailAccountActivated = new Email
                        {
                            From = new EmailAddress("AccountRegistrationDepartment@TSMbank.com", "TSM Bank"),
                            Subject = "Reply On Account Requets By TSM bank",
                            To = new EmailAddress(individual.Email),
                            PlainTextContent = "Your Petition is Approved",
                            HtmlContent = "Your Petition for Activating your account has been" +
                            "APPROVED!!! Thank you for choosing our Bank."
                        };
                        var sendedemailAccountActivated = Email.SendMail(emailAccountActivated);
                        context.SaveChanges();
                        
                    }
                    else if (requestStatus == RequestStatus.Rejected)
                    {
                        request1.Status = requestStatus;
                        var emailRejection = new Email
                        {
                            From = new EmailAddress("PetitionDepartment@TSMbank.com", "TSM Bank"),
                            Subject = "Reply On Bank Account Requets By TSM bank",
                            To = new EmailAddress(individual.Email),
                            PlainTextContent = "Your Petition is rejected",
                            HtmlContent = "Your Petition for BankAccount creation has been REJECTED"
                        };
                        var sendEmailRejection = Email.SendMail(emailRejection);
                        //edw 8a apo8ikevoume an theloume ta email
                        context.SaveChanges();
                        
                    }
                    return RedirectToAction("GetRequests", new { status = RequestStatus.Pending });

                case RequestType.BankAccActivation:
                    var request2 = context.BankAccRequests
                                        .Include(r => r.Individual)
                                        .SingleOrDefault(r => r.Id == Id);
                    var individual2 = context.Individuals
                        .SingleOrDefault(c => c.Id == request2.IndividualId);

                    if (requestStatus == RequestStatus.Approved)
                    {
                        request2.Status = RequestStatus.Processing;
                        context.SaveChanges();
                        return RedirectToAction("NewAccountRequest", "BankAccounts", request2);

                    }else if (requestStatus == RequestStatus.Rejected)
                    {
                        request2.Status = requestStatus;
                        var emailRejection = new Email
                        {
                            From = new EmailAddress("PetitionDepartment@TSMbank.com", "TSM Bank"),
                            Subject = "Reply On Bank Account Requets By TSM bank",
                            To = new EmailAddress(individual2.Email),
                            PlainTextContent = "Your Petition is rejected",
                            HtmlContent = "Your Petition for BankAccount creation has been REJECTED"
                        };
                        var sendEmailRejection = Email.SendMail(emailRejection);
                        context.SaveChanges();
                        
                    }
                    return RedirectToAction("GetRequests", new { status = RequestStatus.Pending });

                case RequestType.CreditCardActivation:
                    break;
                default:
                    break;
            }
            context.SaveChanges();

            return RedirectToAction("GetRequests", new { status = RequestStatus.Pending });
        }
        

    }
}