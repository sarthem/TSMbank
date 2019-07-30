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

        public ActionResult Details(int id, string accountNumber)
        {
            var transaction = unitOfWork.Transactions.GetTransactions(id);//1

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
            var bankAccount = unitOfWork.BankAccounts.GetBankAccountWithTransactions(accountNumber);//2    
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
            //3            
            var userId = User.Identity.GetUserId();
            var individual = unitOfWork.Individuals.GetIndividualWithBankAcc(userId);//4

            var bankAccount = unitOfWork.BankAccounts.GetBankAccountsOfIndividual(individual.Id);//5
                
            var viewModel = new TransactionViewModel()
            {
                Individual = individual,
                BankAccounts = bankAccount.ToList(),                
            };

            return View(viewModel);
        }

        public ActionResult TransferToAccount(TransactionViewModel transactionView)
        {
            var transactionType = unitOfWork.TransactionTypes.GetTransactionType(transactionView.Category);//9
            
            var debitAccount = unitOfWork.BankAccounts.GetJustBankAccount(transactionView.BankAccountId);//6

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

            var creditAccount = unitOfWork.BankAccounts.GetJustBankAccount(creditAccountNumber);//7

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
            //8
            unitOfWork.Transactions.AddTransaction(transaction);
            var transHub = new
            {
                DebitAccountNo = debitAccount.AccountNumber,
                CreditAccountNo = creditAccount.AccountNumber,
                DebitAmount = transactionView.Amount,
                Time = transaction.ValueDateTime
            };

            SignalRHub.GetTransactions(transHub);

            unitOfWork.Complete();            

            return RedirectToAction("Index", new { AccountNumber = transactionView.BankAccountId });
        }
    }
}
//1
//context.Transactions.Where(t => t.TransactionId == id).ToList();

//2    
//context.BankAccounts
//                .Include(a => a.CreditTransactions)
//                .Include(a => a.DebitTransactions)
//                .SingleOrDefault(a => a.AccountNumber == accountNumber);

//3
// var appUser = unitOfWork.Users.GetUser(User.Identity.GetUserId());
// context.Users.Find();

//4
//context.Individuals
//                        .Include(c => c.BankAccounts)
//                        .SingleOrDefault(c => c.Id == appUser.Id);

//5
//context.BankAccounts
//            .Where(c => c.IndividualId == individual.Id);

//6
//context.BankAccounts.SingleOrDefault(ac => ac.AccountNumber == transactionView.BankAccountId);

//7
//context.BankAccounts.SingleOrDefault(ac => ac.AccountNumber == creditAccountNumber);

//8
//context.Transactions.Add(transaction);
//context.SaveChanges();

//9
//context.TransactionTypes.SingleOrDefault(tr => tr.Category == transactionView.Category);
