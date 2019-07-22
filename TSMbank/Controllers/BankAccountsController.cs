using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TSMbank.Models;
using TSMbank.ViewModels;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace TSMbank.Controllers
{

    public class BankAccountsController : Controller
    {
        private ApplicationDbContext context;

        public BankAccountsController()
        {
            context = new ApplicationDbContext();
        }

        // GET: Accounts
        public ActionResult Index()
        {
            return View();
        }


        // GET
        public ActionResult New(string individualId)
        {
            if (individualId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var individual = context.Individuals.SingleOrDefault(c => c.Id == individualId);
            if (individual == null)
                return HttpNotFound();

            var bankAccount = new BankAccount();
            bankAccount.IndividualId = individual.Id;

            var viewModel = new BankAccountFormViewModel()
            {
                BankAccount = bankAccount,
                IndividualFullName = individual.FullName,
                BankAccountTypes = context.BankAccountTypes.ToList(),
            };

            return View("BankAccountForm", viewModel);
        }

        //new method
        public ActionResult NewAccountRequest(BankAccRequest bankAccRequest)
        {
            var individual = context.Individuals.SingleOrDefault(i => i.Id == bankAccRequest.IndividualId);
            if (individual == null)
                return HttpNotFound();

            var accountTypeDiscription = context.BankAccountTypes
                 .SingleOrDefault(a => a.Id == bankAccRequest.BankAccTypeId);
           
            var viewModel = new BankAccountFormViewModel()
            {
                IndividualFullName = individual.FullName,
                BankAccountTypes = context.BankAccountTypes.Where(a => a.Id == accountTypeDiscription.Id).ToList(),
                AccoutTypeDescription = bankAccRequest.BankAccSummury,
                BankAccount = new BankAccount()
                {
                    IndividualId = individual.Id
                }
            };

            return View("BankAccountForm", viewModel);
        }

        // GET 
        public ActionResult Edit(string accountNo)
        {
            if (accountNo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var bankAccount = context.BankAccounts.Include(a => a.Individual).SingleOrDefault(a => a.AccountNumber == accountNo);

            if (bankAccount == null)
                return HttpNotFound();

            var viewModel = new BankAccountFormViewModel()
            {
                BankAccount = bankAccount,
                BankAccountTypes = context.BankAccountTypes.ToList(),
                IndividualFullName = bankAccount.Individual.FullName

            };

            return View("BankAccountForm", viewModel);
        }

        //GET
        public ActionResult Details(string accountNo)
        {
            if (accountNo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var bankAccount = context.BankAccounts
                .Include(a => a.Individual)
                .Include(a => a.BankAccountType)
                .SingleOrDefault(a => a.AccountNumber == accountNo);
            var actype = context.BankAccountTypes.SingleOrDefault(a => a.Id == bankAccount.BankAccountTypeId);
            if (bankAccount == null)
                return HttpNotFound();

            var viewModel = new BankAccountFormViewModel()
            {
                BankAccount = bankAccount,
                BankAccountTypes = context.BankAccountTypes.ToList(),
                IndividualFullName = bankAccount.Individual.FullName,
                AccoutTypeDescription = actype.Summary

            };

            return View("Details", viewModel);
        }

        [HttpPost]
        public ActionResult Save(BankAccount bankAccount)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new BankAccountFormViewModel()
                {
                    BankAccount = bankAccount,
                    IndividualFullName = context.Individuals.SingleOrDefault(c => c.Id == bankAccount.IndividualId).FullName,
                    BankAccountTypes = context.BankAccountTypes.ToList(),
                };
                return View("BankAccountForm", viewModel);
            }

            if (bankAccount.AccountNumber == null)
            {
                bankAccount.AccountNumber = BankAccount.CreateRandomAccountNumber();
                bankAccount.OpenedDate = DateTime.Now;
                bankAccount.StatusUpdatedDateTime = DateTime.Now;                   
                context.BankAccounts.Add(bankAccount);

                //new code start               
                var individual = context.Individuals.SingleOrDefault(c => c.Id == bankAccount.IndividualId);
                var request = context.BankAccRequests.Single(r => r.IndividualId == individual.Id && r.Status == RequestStatus.Processing);
                request.Status = RequestStatus.Approved;
                var emailBankAccount = new EmailInfo
                {
                    From = new EmailAddress("BankAccountDepartment@TSMbank.com", "TSM Bank"),
                    Subject = "Reply On BankAccount Request By TSM bank",
                    To = new EmailAddress(individual.Email),
                    PlainTextContent = "Your Petition has been Approved",
                    HtmlContent = "Your Petition for new BankAccount has been" +
                           "APPROVED!!! Thank you for choosing our Bank."
                };
                var sendedemailBankAccount = EmailInfo.SendMail(emailBankAccount);
                //new code finish
                
            }
            else
            {
                var bankAccountInDb = context.BankAccounts.SingleOrDefault(a => a.AccountNumber == bankAccount.AccountNumber);
                bankAccountInDb.WithdrawalLimit = bankAccount.WithdrawalLimit;
                bankAccountInDb.BankAccountTypeId = bankAccount.BankAccountTypeId;
            }
            context.SaveChanges();
            //return RedirectToAction("Index", "Individuals");
            return RedirectToAction("GetIndividuals", "Individuals");
        }

        [Route("Accounts/ChangeStatus/{individualId}")]
        public ActionResult ChangeStatus(string accountNo)
        {
            if (accountNo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var bankAccount = context.BankAccounts.SingleOrDefault(a => a.AccountNumber == accountNo);

            if (bankAccount == null)
                return HttpNotFound();

            bankAccount.AccountStatus = bankAccount.AccountStatus == AccountStatus.Active ? AccountStatus.Inactive : AccountStatus.Active;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = RoleName.Customer)]
        public ActionResult CheckingAccount()
        {
            var userId = User.Identity.GetUserId();
            var individual = context.Individuals.SingleOrDefault(i => i.Id == userId);
            var viewModel = new CheckingAccApplicationViewModel()
            {
                IndividualStatus = individual.Status,
                CheckingAccountTypes = context.BankAccountTypes.Where(t => t.Description == Description.Checking)
            };
            return View(viewModel);
        }
       
        public ActionResult BankAccountSelection(Description description)
        {
            var userId = User.Identity.GetUserId();
            var individual = context.Individuals.SingleOrDefault(i => i.Id == userId);
            var viewModel = new CheckingAccApplicationViewModel() { IndividualStatus = individual.Status };
            switch (description)
            {
                case Description.Checking:                    
                    return View("CheckingAccount", viewModel);
                case Description.Savings:
                    return View("SavingAccount", viewModel);
                case Description.Term:
                    return View("SavingAccount", viewModel);
                case Description.CreditCard:
                    return View("SavingAccount", viewModel);
                default:
                    return View("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }
    }
}