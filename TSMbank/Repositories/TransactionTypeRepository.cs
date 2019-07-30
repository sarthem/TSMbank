using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.Repositories
{
    public class TransactionTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public TransactionType GetTransactionType(TransactionCategory category)
        {
            return _context.TransactionTypes.SingleOrDefault(tr => tr.Category == category);
        }
    }
}