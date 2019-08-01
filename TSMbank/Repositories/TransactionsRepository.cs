using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;
using System.Data.Entity;

namespace TSMbank.Repositories
{
    public class TransactionsRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Transaction> GetTransaction(int id)
        {
            return _context.Transactions.Where(t => t.TransactionId == id);
        }

        public void AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
        }

        public IEnumerable<Transaction> GetTransactions(string bankAccNo)
        {
            return _context.Transactions
                .Include(t => t.DebitAccount.BankAccountType)
                .Include(t => t.CreditAccount.BankAccountType)
                .Where(t => t.CreditAccountNo == bankAccNo || t.DebitAccountNo == bankAccNo);
        }

        

    }
}