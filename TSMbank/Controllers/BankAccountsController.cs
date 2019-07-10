using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TSMbank.Models;
using TSMbank.ViewModels;
using System.Data.Entity;

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


        [Route("bankAccounts/New/{individualId}")]
        public ActionResult New(int? individualId)
        {
            if (!individualId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var individual = context.Individuals.SingleOrDefault(c => c.Id == individualId);
            if (individual == null)
                return HttpNotFound();

            var account = new BankAccount();
            account.IndividualId = individual.Id;

            var viewModel = new BankAccountFormViewModel()
            {
                BankAccount = account,
                IndividualFullName = individual.FullName,
                BankAccountTypes = context.BankAccountTypes.ToList(),
            };

            return View("BankAccountForm",viewModel);
        }

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
                return View("AccountForm", viewModel);
            }

            if (bankAccount.AccountNumber == null)
            {
                bankAccount.AccountNumber = BankAccount.CreateRandomAccountNumber();
                bankAccount.OpenedDate = DateTime.Now;
                bankAccount.StatusUpdatedDateTime = DateTime.Now;
                context.BankAccounts.Add(bankAccount);
            }
            else
            {
                var bankAccountInDb = context.BankAccounts.SingleOrDefault(a => a.AccountNumber == bankAccount.AccountNumber);
                bankAccountInDb.WithdrawalLimit = bankAccount.WithdrawalLimit;
                bankAccountInDb.BankAccountTypeId = bankAccount.BankAccountTypeId;
            }
            context.SaveChanges();
            return RedirectToAction("Index", "Individuals");
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

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }
    }
}