using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TSMbank.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }

        public ApplicationDbContext()
            : base("TSMbankDBContext", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Account>()
                        .HasMany(a => a.CreditTransactions)
                        .WithRequired(tr => tr.CreditAccount)
                        .HasForeignKey(t => t.CreditAccountNo)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
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