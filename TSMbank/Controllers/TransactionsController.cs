using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSMbank.Hubs;
using TSMbank.Models;
using TSMbank.Persistance;
using TSMbank.ViewModels;

namespace TSMbank.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UnitOfWork unitOfWork;


        public TransactionsController()
        {
            context = new ApplicationDbContext();
            unitOfWork = new UnitOfWork(context);
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }

        public ActionResult GetTransactions()
        {
            var transaction = context.Transactions.OrderByDescending(t => t.ValueDateTime).ToList();

            return View(transaction);
        }



        public ActionResult Details(int id, string accountNumber)
        {
            var transaction = unitOfWork.Transactions.GetTransaction(id);

            var viewModel = new TransactionsDetailsViewModel()
            {
                Transactions = transaction,
                AccountNumber = accountNumber
            };
            return View(viewModel);
        }

        // GET: Transactions
        public ActionResult Index(string accountNumber)
        {
            var bankAccount = unitOfWork.BankAccounts.GetBankAccountWithTransactions(accountNumber);
            var transactions = bankAccount.DebitTransactions.Concat(bankAccount.CreditTransactions);
            var orderedTransactions = transactions.OrderByDescending(t => t.ValueDateTime);

            var viewModel = new TransactionsDetailsViewModel()
            {
                Transactions = orderedTransactions,
                AccountNumber = accountNumber
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult TransferMoney(string accountNumber)
        {
            var userId = User.Identity.GetUserId();
            var customerBankAccs = unitOfWork.BankAccounts.GetCheckingAndSavingsBankAccs(userId);

            if (accountNumber != null)
                customerBankAccs = customerBankAccs.Where(a => a.AccountNumber == accountNumber);

            var viewModel = new TransferMoneyViewModel()
            {
                CustomerBankAccs = customerBankAccs,
                TransactionCategory = TransactionCategory.MoneyTransfer
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult TransferMoney(TransferMoneyViewModel viewModel)
        {
            var userId = User.Identity.GetUserId();

            if (!ModelState.IsValid)
            {
                viewModel.CustomerBankAccs = unitOfWork.BankAccounts.GetCheckingAndSavingsBankAccs(userId);
                return View("TransferMoney", viewModel);
            }

            var transactions = new List<Transaction>();
            var transactionType = unitOfWork.TransactionTypes.GetTransactionType(viewModel.TransactionCategory);
            var debitAccount = unitOfWork.BankAccounts.GetJustBankAccount(viewModel.DebitAccNo);
            var creditAccNo = viewModel.CreditAccNo ?? viewModel.CreditAccIban.Substring(10);
            var creditAccount = unitOfWork.BankAccounts.GetJustBankAccount(creditAccNo);

            if (creditAccount == null)
            {
                viewModel.CustomerBankAccs = unitOfWork.BankAccounts.GetCheckingAndSavingsBankAccs(userId);
                viewModel.ErrorMessage = "Credit Bank Account could not be found.";
                return View("TransferMoney", viewModel);
            }

            if (!debitAccount.InitiateTransaction(creditAccount, viewModel.Amount.Value, transactionType, null, out transactions))
            {
                viewModel.CustomerBankAccs = unitOfWork.BankAccounts.GetCheckingAndSavingsBankAccs(userId);
                viewModel.ErrorMessage = "Could not complete transaction.";
                return View("TransferMoney", viewModel);
            }

            foreach (var transaction in transactions)
            {
                unitOfWork.Transactions.AddTransaction(transaction);
            }
            unitOfWork.Complete();

            var transHub = new
            {
                DebitAccountNo = debitAccount.AccountNumber,
                DebitBalance = debitAccount.Balance,
                CreditAccountNo = creditAccount.AccountNumber,
                CreditBalance = creditAccount.Balance,
                DebitAmount = transactions[0].DebitAmount,
                Time = transactions[0].ValueDateTime.ToLongTimeString(),
                Date = transactions[0].ValueDateTime.ToLongDateString()
            };

            SignalHub.GetTransactions(transHub);
            return RedirectToAction("Index", new { AccountNumber = debitAccount.AccountNumber });
        }

        public ActionResult Payment()
        {
            var userId = User.Identity.GetUserId();
            var customerBankAccs = context.BankAccounts
                    .Include(ba => ba.BankAccountType)
                    .Include(ba => ba.Card)
                    .Where(ba => ba.IndividualId == userId)
                    .ToList();

            var publicAccounts = context.BankAccounts
                    .Include(ba => ba.BankAccountType)
                    .Where(pa => pa.BankAccountType.Description == Description.PublicServices)
                    .ToList();

            var viewModel = new PaymentFormViewModel()
            {
                CustomerBankAccs = customerBankAccs,
                TransactionCategory = TransactionCategory.Payment,
                PublicPaymentAccs = publicAccounts
            };

            return View(viewModel);
        }
    }
}

