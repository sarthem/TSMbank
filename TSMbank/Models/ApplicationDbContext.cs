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
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<BankAccount> Accounts { get; set; }
        public DbSet<BankAccountType> AccountTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }


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

            modelBuilder.Entity<Customer>()
                        .HasRequired(c => c.PrimaryAddress)
                        .WithMany()
                        .HasForeignKey(c => c.PrimaryAddressId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                        .HasOptional(c => c.SecondaryAddress)
                        .WithMany()
                        .HasForeignKey(c => c.SecondaryAddressId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                        .HasMany(c => c.Phones)
                        .WithRequired(p => p.Customer)
                        .HasForeignKey(p => p.CustomerId)
                        .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}