using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TSMbank.Dtos;
using TSMbank.Models;
using TSMbank.Persistance;

namespace TSMbank.Controllers.api
{
    public class TransactionsController : ApiController
    {
        private readonly ApplicationDbContext context;
        private readonly UnitOfWork unitOfWork;

        public TransactionsController()
        {
            context = new ApplicationDbContext();
            unitOfWork = new UnitOfWork(context);
        }


        public IHttpActionResult GetTransactions()
        {
            var transactions = context.Transactions
                              .Include(t => t.CreditAccount)
                              .Include(t => t.DebitAccount).ToList();
            return Ok(transactions);
        }

        [HttpPost]
        public IHttpActionResult MakePayment(TransactionDto transactionDto)
        {
            var transactions = new List<Transaction>();
            var userId = User.Identity.GetUserId();
            var transactionType = unitOfWork.TransactionTypes.GetTransactionType(transactionDto.Category);
            var debitAccount = unitOfWork.BankAccounts.GetJustBankAccount(transactionDto.DebitAccNo);
            var creditAccount = unitOfWork.BankAccounts.GetJustBankAccount(transactionDto.CreditAccNo);
            var tsmBankAcc = unitOfWork.BankAccounts.GetTsmBankAcc();

            if (creditAccount == null || debitAccount == null)
            {
                return NotFound();
            }

            if (!debitAccount.InitiateTransaction(creditAccount, transactionDto.Amount, transactionType, tsmBankAcc, out transactions))
            {
                return BadRequest("Transaction could not be completed.");
            }

            foreach (var transaction in transactions)
            {
                unitOfWork.Transactions.AddTransaction(transaction);
            }
            unitOfWork.Complete();
            
            return Ok();
        }

    }
}
