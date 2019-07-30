using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.Repositories
{
    public class RequestRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Request> GetRequestsByStatus(RequestStatus status)
        {
            return _context.Requests.Where(r => r.Status == status);
        }

        public void AddRequest(Request request)
        {
            _context.Requests.Add(request);
        }

        public Request GetRequest(int id)
        {
            return _context.Requests
                            .Include(r => r.Individual.BankAccounts)
                            .SingleOrDefault(r => r.Id == id);
        }

        public Request GetUserAccRequest(int id)
        {
            return _context.Requests
                            .Include(r => r.Individual.Phones)
                            .Include(r => r.Individual.PrimaryAddress)
                            .SingleOrDefault(r => r.Id == id);
        }
        
    }
}