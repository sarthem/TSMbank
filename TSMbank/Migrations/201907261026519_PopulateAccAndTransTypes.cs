namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using TSMbank.Models;

    public partial class PopulateAccAndTransTypes : DbMigration
    {
        public override void Up()
        {
            Sql($"INSERT INTO dbo.BankAccountTypes (Id,Description, InterestRate, PeriodicFee, Summary) VALUES ({BankAccountType.CheckingPremium}, 0, 0.5, 25, 'Checking Premium')");
            Sql($"INSERT INTO dbo.BankAccountTypes (Id,Description, InterestRate, PeriodicFee, Summary) VALUES ({BankAccountType.SavingsBasic}, 1, 3.5, 50, 'Savings Basic')");
            Sql($"INSERT INTO dbo.BankAccountTypes (Id,Description, InterestRate, PeriodicFee, Summary) VALUES ({BankAccountType.TermBasic}, 2, 6.5, 100, 'Term Basic')");
            Sql($"INSERT INTO dbo.BankAccountTypes (Id,Description, InterestRate, PeriodicFee, Summary) VALUES ({BankAccountType.CheckingBasic}, 0, 0, 25, 'Checking Basic')");
            Sql($"INSERT INTO dbo.BankAccountTypes (Id,Description, InterestRate, PeriodicFee, Summary) VALUES ({BankAccountType.TSMVisaClassic}, 3, 16, 30, 'TSM Visa Classic')");
            Sql($"INSERT INTO dbo.BankAccountTypes (Id,Description, InterestRate, PeriodicFee, Summary) VALUES ({BankAccountType.SavingsPremium}, 1, 5, 70, 'Savings Premium')");
            Sql($"INSERT INTO dbo.BankAccountTypes (Id,Description, InterestRate, PeriodicFee, Summary) VALUES ({BankAccountType.PublicServices}, 4, 0, 1000, 'Public Services')");

            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (1, 0, 1, 'Deposit')");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (2, 1, 1, 'Withdrawl')");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (3, 2, 2, 'Payment')");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (4, 3, 25, 'Cancellation')");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (5, 4, 0, 'Money Transfer')");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (6, 5, 0, 'Purchase')");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (7, 6, 0, 'Interest Fee')");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (8, 7, 0, 'Overdue Fee')");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (9, 8, 0, 'Bank Commission')");
        }

        public override void Down()
        {
        }
    }
}
