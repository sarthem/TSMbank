using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSMbank.Models;
using TSMbank.ViewModels;

namespace TSMbank.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext context;
        

        public TransactionsController()
        {
            context = new ApplicationDbContext();
            
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }




        public ActionResult Details(int id, string accountNumber)
        {
            var transaction = context.Transactions.Where(t => t.TransactionId == id).ToList();
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
            //var appUser = context.Users.Find(User.Identity.GetUserId());

            //var creditTransaction = context.Transactions
            //    .Include(customer => customer.CreditAccount)
            //    .Where(c => c.CreditAccount.Customer.ApplicationUserId == appUser.Id)
            //    .Include(c => c.CreditAccount.Customer)
            //    .Where(c => c.CreditAccount.AccountNumber == accountNumber).ToList();

            //var debitTransaction = context.Transactions
            //    .Include(customer => customer.DebitAccount)
            //    .Where(c => c.DebitAccount.Customer.ApplicationUserId == appUser.Id)
            //    .Include(c => c.DebitAccount.Customer)
            //    .Where(c => c.DebitAccount.AccountNumber == accountNumber).ToList();

            var bankAccount = context.BankAccounts
                .Include(a => a.CreditTransactions)
                .Include(a => a.DebitTransactions)
                .SingleOrDefault(a => a.AccountNumber == accountNumber);




            //var transactions = creditTransaction.Concat(debitTransaction);

            var transactions = bankAccount.DebitTransactions.Concat(bankAccount.CreditTransactions);
            var orderedTransactions = transactions.OrderByDescending(t => t.ValueDateTime);
           


            var viewModel = new TransactionsDetailsViewModel()
            {
                Transactions = orderedTransactions,
                AccountNumber = accountNumber
            };

            return View(viewModel);
        }


        public ActionResult Deposit()
        {
            var appUser = context.Users.Find(User.Identity.GetUserId());
            var customer = context.Customers.Include(c => c.Accounts).SingleOrDefault(c => c.ApplicationUserId == appUser.Id);
            var bankAccount = context.BankAccounts.Where(c => c.CustomerId == customer.Id);
            var viewModel = new TransactionDepositViewForm()
            {
                Customer = customer,
                Accounts = bankAccount.ToList()
            };


            return View(viewModel);
        }



        public ActionResult TransferToAccount(TransactionDepositViewForm transactionDepositView)
        {
            string debitNumber = "";
            string debitAccountNumber = "";
            if ( transactionDepositView.DebitIBAN != null)
            {
                debitNumber = transactionDepositView.DebitIBAN;
                debitAccountNumber = debitNumber.Substring(10);
            }
            else
            {
                debitAccountNumber = transactionDepositView.DebitAccount;
            }
            
            var creditAccount = context.BankAccounts.SingleOrDefault(ac => ac.AccountNumber == transactionDepositView.AccountId);
            var debitAccount = context.BankAccounts.SingleOrDefault(ac => ac.AccountNumber == debitAccountNumber);
                        
            var transaction = new Transaction()
            {
                ValueDateTime = DateTime.Now,

                CreditAccount = creditAccount,
                CreditAccountNo = creditAccount.AccountNumber,
                CreditIBAN = creditAccount.IBAN,
                CreditAccountBalance = creditAccount.Balance,
                CreditAccountCurrency = "EURO",
                CreditAmount = transactionDepositView.Amount,
                CreditAccountBalanceAfterTransaction = creditAccount.Balance - transactionDepositView.Amount,

                DebitAccount = debitAccount,
                DebitAccountNo = transactionDepositView.DebitAccount,
                DebitIBAN = debitAccount.IBAN,
                DebitAccountBalance = debitAccount.Balance,
                DebitAccountCurrency = "EURO",
                DebitAmount = transactionDepositView.Amount,
                DebitAccountBalanceAfterTransaction = debitAccount.Balance + transactionDepositView.Amount,

                TypeId = 1,
                ApprovedFromBankManager = true,
                PendingForApproval = false,
                TransactionApprovedReview = TransactionApprovedReview.Approve,
                IsCompleted = true,
                
            };
            creditAccount.Balance = transaction.CreditAccountBalanceAfterTransaction;
            debitAccount.Balance = transaction.DebitAccountBalanceAfterTransaction;

            context.Transactions.Add(transaction);
            context.SaveChanges();

            return RedirectToAction("Index", new { AccountNumber = transactionDepositView.AccountId });//, transactionDepositView.AccountId
        }
    }
}