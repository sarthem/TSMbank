﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.Repositories
{
    public class BankAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public BankAccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public BankAccount GetJustBankAccount(string accountNumber)
        {
            return _context.BankAccounts.SingleOrDefault(ac => ac.AccountNumber == accountNumber);
        }

        public BankAccount GetBankAccount(string accountNumber)
        {
            return _context.BankAccounts
                            .Include(a => a.Individual)
                            .Include(a => a.BankAccountType)
                            .SingleOrDefault(a => a.AccountNumber == accountNumber);
        }

        public IQueryable<BankAccount> GetBankAccountsOfIndividual(string id)
        {
            return _context.BankAccounts.Where(c => c.IndividualId == id);
        }

        public void AddBankAccount(BankAccount bankAccount)
        {
            _context.BankAccounts.Add(bankAccount);
        }

        public BankAccount GetBankAccountWithTransactions(string accountNumber)
        {
            return _context.BankAccounts
                            .Include(a => a.CreditTransactions)
                            .Include(a => a.DebitTransactions)
                            .SingleOrDefault(a => a.AccountNumber == accountNumber);
        }

        public IEnumerable<BankAccount> GetCheckingAndSavingsBankAccs(string id)
        {
            return _context.BankAccounts
                    .Include(ba => ba.BankAccountType)
                    .Where(ba => ba.IndividualId == id && (ba.BankAccountType.Description == Description.Checking
                        || ba.BankAccountType.Description == Description.Savings))
                    .ToList();
        }

    }
}