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
    public class AccountsController : Controller
    {
        private ApplicationDbContext context;

        public AccountsController()
        {
            context = new ApplicationDbContext();
        }

        // GET: Accounts
        public ActionResult Index()
        {
            return View();
        }

        [Route("Accounts/New/{customerId}")]
        public ActionResult New(int? customerId)
        {
            if (!customerId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var customer = context.Customers.SingleOrDefault(c => c.Id == customerId);
            if (customer == null)
                return HttpNotFound();

            var account = new Account();
            account.CustomerId = customer.Id;

            var viewModel = new AccountFormViewModel()
            {
                Account = account,
                CustomerFullName = customer.FullName,
                AccountTypes = context.AccountTypes.ToList(),
            };

            return View("AccountForm",viewModel);
        }

        public ActionResult Edit(string accountNo)
        {
            if (accountNo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var account = context.Accounts.Include(a => a.Customer).SingleOrDefault(a => a.AccountNumber == accountNo);

            if (account == null)
                return HttpNotFound();

            var viewModel = new AccountFormViewModel()
            {
                Account = account,
                AccountTypes = context.AccountTypes.ToList(),
                CustomerFullName = account.Customer.FullName,
            };

            return View("AccountForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(Account account)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new AccountFormViewModel()
                {
                    Account = account,
                    CustomerFullName = context.Customers.SingleOrDefault(c => c.Id == account.CustomerId).FullName,
                    AccountTypes = context.AccountTypes.ToList(),
                };
                return View("AccountForm", viewModel);
            }

            if (account.AccountNumber == null)
            {
                account.AccountNumber = Account.CreateRandomAccountNumber();
                account.OpenedDate = DateTime.Now;
                account.StatusUpdatedDateTime = DateTime.Now;
                context.Accounts.Add(account);
            }
            else
            {
                var accountInDb = context.Accounts.SingleOrDefault(a => a.AccountNumber == account.AccountNumber);
                accountInDb.WithdrawalLimit = account.WithdrawalLimit;
                accountInDb.AccountTypeId = account.AccountTypeId;
            }
            context.SaveChanges();
            return RedirectToAction("Index", "Customers");
        }

        [Route("Accounts/ChangeStatus/{customerId}")]
        public ActionResult ChangeStatus(string accountNo)
        {
            if (accountNo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var account = context.Accounts.SingleOrDefault(a => a.AccountNumber == accountNo);

            if (account == null)
                return HttpNotFound();

            account.AccountStatus = account.AccountStatus == AccountStatus.Active ? AccountStatus.Inactive : AccountStatus.Active;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }
    }
}