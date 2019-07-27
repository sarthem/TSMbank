﻿using Microsoft.AspNet.Identity;
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
            var bankAccount = context.BankAccounts
                .Include(a => a.CreditTransactions)
                .Include(a => a.DebitTransactions)
                .SingleOrDefault(a => a.AccountNumber == accountNumber);

            var transactions = bankAccount.DebitTransactions.Concat(bankAccount.CreditTransactions);
            var orderedTransactions = transactions.OrderByDescending(t => t.ValueDateTime);

            var viewModel = new TransactionsDetailsViewModel()
            {
                Transactions = orderedTransactions,
                AccountNumber = accountNumber
            };

            return View(viewModel);
        }

        // Get 
        public ActionResult Deposit()
        {
            var appUser = context.Users.Find(User.Identity.GetUserId());
            var individual = context.Individuals.Include(c => c.BankAccounts).SingleOrDefault(c => c.Id == appUser.Id);
            var bankAccount = context.BankAccounts.Where(c => c.IndividualId == individual.Id);
            var viewModel = new TransactionViewModel()
            {
                Individual = individual,
                BankAccounts = bankAccount.ToList(),
                
            };

            return View(viewModel);
        }

        public ActionResult TransferToAccount(TransactionViewModel transactionView)
        {
            var transactionType = context.TransactionTypes.SingleOrDefault(tr => tr.Category == transactionView.Category);

            var debitAccount = context.BankAccounts.SingleOrDefault(ac => ac.AccountNumber == transactionView.BankAccountId);

            string creditNumber = "";
            string creditAccountNumber = "";
            if ( transactionView.CreditIBAN != null)
            {
                creditNumber = transactionView.CreditIBAN;
                creditAccountNumber = creditNumber.Substring(10);
            }
            else
            {
                creditAccountNumber = transactionView.CreditAccount;
            }            
            var creditAccount = context.BankAccounts.SingleOrDefault(ac => ac.AccountNumber == creditAccountNumber);


            if(creditAccount == null)
            {
                var viewModel = new ErrorTransactionMessage()
                {
                    message = "no credit iban"
                };
                return View("TransferToAccountERROR", viewModel);
            }
            else if(transactionView.Amount > debitAccount.Balance)
            {
                var viewModel = new ErrorTransactionMessage()
                {
                    message = "no balance"
                };
                return View("TransferToAccountERROR", viewModel);
            }


            var transaction = new Transaction()
            {
                ValueDateTime = DateTime.Now,
                //from
                DebitAccount = debitAccount,
                DebitAccountNo = debitAccount.AccountNumber,
                DebitIBAN = debitAccount.IBAN,
                DebitAccountBalance = debitAccount.Balance,
                DebitAccountCurrency = "EURO",
                DebitAmount = transactionView.Amount,
                DebitAccountBalanceAfterTransaction = 
                debitAccount.Balance 
                - transactionView.Amount
                - transactionType.Fee,
                //to
                CreditAccount = creditAccount,
                CreditAccountNo = creditAccount.AccountNumber,
                CreditIBAN = creditAccount.IBAN,
                CreditAccountBalance = creditAccount.Balance,
                CreditAccountCurrency = "EURO",
                CreditAmount = transactionView.Amount,

                CreditAccountBalanceAfterTransaction = creditAccount.Balance + transactionView.Amount ,

                TypeId = transactionType.Id,
                ApprovedFromBankManager = true,
                PendingForApproval = false,
                TransactionApprovedReview = TransactionApprovedReview.Approve,
                IsCompleted = true,
                Type = transactionType,
                
                
            };
            
            creditAccount.Balance = transaction.CreditAccountBalanceAfterTransaction;
            debitAccount.Balance = transaction.DebitAccountBalanceAfterTransaction;                        

            context.Transactions.Add(transaction);
            context.SaveChanges();

            return RedirectToAction("Index", new { AccountNumber = transactionView.BankAccountId });
        }

        public ActionResult MakePayment()
        {
            var userId = User.Identity.GetUserId();
            var appUser = context.Users.Include(a => a.Individual).Include(a => a.Individual.BankAccounts).SingleOrDefault(a => a.Id == userId);

            var publicAccounts = context.BankAccounts.Where(pa => pa.BankAccountType.Description == Description.PublicServices).ToList();

            var viewModel = new PaymentViewFormModel()
            {
                Individual = appUser.Individual,
                BankAccounts = appUser.Individual.BankAccounts.ToList(),
                Category = TransactionCategory.Payment,
                PublicServiceType = publicAccounts

            };

            return View(viewModel);
        }
    }
}