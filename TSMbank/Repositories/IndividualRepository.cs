using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.Repositories
{
    public class IndividualRepository
    {
        private readonly ApplicationDbContext _context;

        public IndividualRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Individual GetIndividual(string userId)
        {
            return _context.Individuals
                            .Include(c => c.Phones)
                            .Include(c => c.PrimaryAddress)
                            .Include(c => c.BankAccounts)
                            .SingleOrDefault(c => c.Id == userId);
        }

        public IEnumerable<Individual> GetIndividuals()
        {
            return _context.Individuals
                                .Include(c => c.Phones)
                                .Include(c => c.PrimaryAddress)
                                .Include(c => c.BankAccounts).ToList();
        }

        public Individual GetIndividualWithAddressAndPhone(string userId)
        {
            return _context.Individuals
                            .Include(c => c.Phones)
                            .Include(c => c.PrimaryAddress)
                            .Include(c => c.SecondaryAddress)
                            .SingleOrDefault(c => c.Id == userId);
        }

    }
}

