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
            //    .Include(individual => individual.CreditAccount)
            //    .Where(c => c.CreditAccount.Individual.ApplicationUserId == appUser.Id)
            //    .Include(c => c.CreditAccount.Individual)
            //    .Where(c => c.CreditAccount.AccountNumber == accountNumber).ToList();

            //var debitTransaction = context.Transactions
            //    .Include(individual => individual.DebitAccount)
            //    .Where(c => c.DebitAccount.Individual.ApplicationUserId == appUser.Id)
            //    .Include(c => c.DebitAccount.Individual)
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
            var individual = context.Individuals.Include(c => c.BankAccounts).SingleOrDefault(c => c.ApplicationUserId == appUser.Id);
            var bankAccount = context.BankAccounts.Where(c => c.IndividualId == individual.Id);
            var viewModel = new TransactionDepositViewForm()
            {
                Individual = individual,
                BankAccounts = bankAccount.ToList()
            };


            return View(viewModel);
        }



        public ActionResult TransferToAccount(TransactionDepositViewForm transactionDepositView)
        {

            var debitAccount = context.BankAccounts.SingleOrDefault(ac => ac.AccountNumber == transactionDepositView.BankAccountId);

            string creditNumber = "";
            string creditAccountNumber = "";
            if ( transactionDepositView.CreditIBAN != null)
            {
                creditNumber = transactionDepositView.CreditIBAN;
                creditAccountNumber = creditNumber.Substring(10);
            }
            else
            {
                creditAccountNumber = transactionDepositView.CreditAccount;
            }            
            var creditAccount = context.BankAccounts.SingleOrDefault(ac => ac.AccountNumber == creditAccountNumber);
                        
            var transaction = new Transaction()
            {
                ValueDateTime = DateTime.Now,
                //from
                DebitAccount = debitAccount,
                DebitAccountNo = debitAccount.AccountNumber,
                DebitIBAN = debitAccount.IBAN,
                DebitAccountBalance = debitAccount.Balance,
                DebitAccountCurrency = "EURO",
                DebitAmount = transactionDepositView.Amount,
                DebitAccountBalanceAfterTransaction = debitAccount.Balance - transactionDepositView.Amount,
                //to
                CreditAccount = creditAccount,
                CreditAccountNo = transactionDepositView.CreditAccount,
                CreditIBAN = creditAccount.IBAN,
                CreditAccountBalance = creditAccount.Balance,
                CreditAccountCurrency = "EURO",
                CreditAmount = transactionDepositView.Amount,
                CreditAccountBalanceAfterTransaction = creditAccount.Balance + transactionDepositView.Amount,

               

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

            return RedirectToAction("Index", new { AccountNumber = transactionDepositView.BankAccountId });
        }
    }
}