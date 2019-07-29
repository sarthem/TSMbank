﻿using System;
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

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Individuals = new IndividualRepository(context);
            BankAccounts = new BankAccountRepository(context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}


  