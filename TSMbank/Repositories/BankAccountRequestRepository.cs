using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.Repositories
{
    public class BankAccountRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public BankAccountRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public BankAccRequest GetBankAccRequest(int id)
        {
            return _context.BankAccRequests
                                        .Include(r => r.Individual)
                                        .Include(r => r.BankAccType)
                                        .SingleOrDefault(r => r.Id == id);
        }

        public BankAccRequest GetBankAccountRequestWithStatus(string userId, byte id)
        {
            return _context.BankAccRequests
                            .Include(r => r.BankAccType)
                            .SingleOrDefault(r => r.IndividualId == userId && r.BankAccTypeId == id
                            && (r.Status == RequestStatus.Pending || r.Status == RequestStatus.Approved));
        }

        public BankAccRequest GetBankAccRequestByStatus(string userId, RequestStatus status)
        {
            return _context.BankAccRequests
                                .Include(r => r.Individual)
                                .Single(r => r.IndividualId == userId
                                && r.Status == RequestStatus.Processing);
        }

        public void AddBankAccountRequest(BankAccRequest bankAccRequest)
        {
            _context.BankAccRequests.Add(bankAccRequest);
        }

    }
}