using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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

    }
}
