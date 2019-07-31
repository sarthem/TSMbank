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
                            .Include(c => c.BankAccounts.Select(ba => ba.Card))
                            .Include(i => i.BankAccounts.Select(ba => ba.BankAccountType))
                            .SingleOrDefault(c => c.Id == userId);
        }
        public Individual GetIndividualWithBankAccs(string id)
        {
            return _context.Individuals
                            .Include(c => c.BankAccounts)
                            .SingleOrDefault(c => c.Id == id);
        }

        public Individual GetIndividualWithMTBankAccs(string id)
        {
            return _context.Individuals
                            .Include(i => i.BankAccounts
                                .Where(ba => ba.BankAccountType.Description == Description.Checking
                                    || ba.BankAccountType.Description == Description.Savings).Select(ba => ba.BankAccountType))
                            .SingleOrDefault(c => c.Id == id);
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

        public Individual GetJustIndividual(string userId)
        {
            return _context.Individuals
                        .SingleOrDefault(u => u.Id == userId);
        }

        public void AddIndividual(Individual individual)
        {
             _context.Individuals.Add(individual);
        }

       
    }

    
}

