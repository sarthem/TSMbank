namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class populateData : DbMigration
    {
        public override void Up()
        {

            Sql("INSERT INTO dbo.BankAccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (0, 0.5, 25, 'Checking Gold')");
            Sql("INSERT INTO dbo.BankAccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (1, 3.5, 50, 'Savings Basic')");
            Sql("INSERT INTO dbo.BankAccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (2, 6.5, 100, 'Term Basic')");
            Sql("INSERT INTO dbo.BankAccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (0, 0, 25, 'Checking Basic')");
            Sql("INSERT INTO dbo.BankAccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (3, 16, 30, 'Credit Card')");

            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (1, 0, 1, 'Deposit')");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (2, 1, 1, 'Withdrawl')");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (3, 2, 2, 'Payment')");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (4, 3, 25, 'Cancellation')");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (5, 4, 0, 'Money Transfer')");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (6, 5, 0, 'Purchase')");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (7, 6, 0, 'Interest Fee')");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee, Description) VALUES (8, 7, 0, 'Overdue Fee')");
        }
        
        public override void Down()
        {
        }
    }
}
