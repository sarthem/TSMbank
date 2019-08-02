using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TSMbank.Dtos;
using TSMbank.Hubs;
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


        //public IHttpActionResult GetTransactions()
        //{
        //    var transactions = context.Transactions
        //                      .Include(t => t.CreditAccount)
        //                      .Include(t => t.DebitAccount).ToList();
        //    return Ok(transactions);
        //}

        [HttpPost]
        public IHttpActionResult MakePayment(TransactionInfoDto transactionDto)
        {
            var transactions = new List<Transaction>();
            var userId = User.Identity.GetUserId();
            var transactionType = unitOfWork.TransactionTypes.GetTransactionType(transactionDto.Category);
            var debitAccount = unitOfWork.BankAccounts.GetJustBankAccount(transactionDto.DebitAccNo);
            var creditAccount = unitOfWork.BankAccounts.GetJustBankAccount(transactionDto.CreditAccNo);
            var tsmBankAcc = unitOfWork.BankAccounts.GetTsmBankAcc();

            if (creditAccount == null || debitAccount == null)
                return NotFound();

            if (!debitAccount.InitiateTransaction(creditAccount, transactionDto.Amount, transactionType, tsmBankAcc, out transactions))
                return BadRequest("Transaction could not be completed.");

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
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult FetchTransactions(string bankAccNo)
        {
            if (bankAccNo == null)
                return BadRequest();

            List<TransactionViewModelDto> dtos = new List<TransactionViewModelDto>();
            var bankAcc = unitOfWork.BankAccounts.FetchBankAccountWithTransactions(bankAccNo);

            if (bankAcc == null)
                return NotFound();

            var transactions = bankAcc.DebitTransactions.Concat(bankAcc.CreditTransactions).Where(t => t.Type.Category != TransactionCategory.Commision);

            foreach (var transaction in transactions)
            {
                dtos.Add(new TransactionViewModelDto
                {
                    Amount = transaction.GetFinancialType(bankAcc) == "Debit" ? -transaction.DebitAmount : transaction.CreditAmount,
                    Category = transaction.Type.Description,
                    FinancialType = transaction.GetFinancialType(bankAcc),
                    RelatedAccInfo = transaction.RelatedAccInfo(bankAcc),
                    ValueDate = transaction.ValueDateTime
                });
            }

            return Ok(dtos);
        }

    }
}
