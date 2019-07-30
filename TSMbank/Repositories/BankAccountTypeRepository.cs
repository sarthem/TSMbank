using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.Repositories
{
    public class BankAccountTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public BankAccountTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public BankAccountType GetBankAccountType(byte bankAccountTypeId)
        {
            return _context.BankAccountTypes
                            .SingleOrDefault(a => a.Id == bankAccountTypeId);
        }

        public IEnumerable<BankAccountType> GetBankAccountTypes()
        {
            return _context.BankAccountTypes.ToList();
        }

        public IQueryable<BankAccountType> BankAccountType()
        {
            return _context.BankAccountTypes;
        }
    }
}