using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;
using TSMbank.Repositories;

namespace TSMbank.Persistance
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IndividualRepository Individuals { get; private set; }
        public BankAccountRepository BankAccounts { get; private set; }
        public RequestRepository Requests { get; private set; }
        public ApplicationUserRepository Users { get; private set; }
        public BankAccountTypeRepository BankAccountTypes { get; private set; }
        public BankAccountRequestRepository BankAccountRequests { get; private set; }
        public TransactionsRepository Transactions { get; private set; }
        public TransactionTypeRepository TransactionTypes { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Individuals = new IndividualRepository(context);
            BankAccounts = new BankAccountRepository(context);
            Requests = new RequestRepository(context);
            Users = new ApplicationUserRepository(context);
            BankAccountTypes = new BankAccountTypeRepository(context);
            BankAccountRequests = new BankAccountRequestRepository(context);
            Transactions = new TransactionsRepository(context);
            TransactionTypes = new TransactionTypeRepository(context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}


  