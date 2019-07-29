    using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TSMbank.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Individual> Individuals { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankAccountType> BankAccountTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<BankAccRequest> BankAccRequests { get; set; }
        public DbSet<CardRequest> CardRequests { get; set; }
        public DbSet<Card> Cards { get; set; }

        public ApplicationDbContext()
            : base("TSMbankDBContext", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<BankAccount>()
                        .HasMany(a => a.CreditTransactions)
                        .WithRequired(tr => tr.CreditAccount)
                        .HasForeignKey(t => t.CreditAccountNo)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<BankAccount>()
                        .HasMany(a => a.DebitTransactions)
                        .WithRequired(tr => tr.DebitAccount)
                        .HasForeignKey(tr => tr.DebitAccountNo)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Individual>()
                        .HasRequired(c => c.PrimaryAddress)
                        .WithMany()
                        .HasForeignKey(c => c.PrimaryAddressId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Individual>()
                        .HasOptional(c => c.SecondaryAddress)
                        .WithMany()
                        .HasForeignKey(c => c.SecondaryAddressId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Individual>()
                        .HasMany(c => c.Phones)
                        .WithRequired(p => p.Individual)
                        .HasForeignKey(p => p.IndividualId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<BankAccount>()
                        .HasOptional(a => a.Card)
                        .WithRequired(c => c.BankAccount)
                        .WillCascadeOnDelete(false);

            //modelBuilder.Entity<BankAccountType>()
            //            .Property(t => t.Id)
            //            .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}