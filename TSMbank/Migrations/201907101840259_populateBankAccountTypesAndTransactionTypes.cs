namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class populateBankAccountTypesAndTransactionTypes : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.BankAccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (1, 0.5, 25, 'Checking Gold')");
            Sql("INSERT INTO dbo.BankAccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (2, 3.5, 50, 'Savings Basic')");
            Sql("INSERT INTO dbo.BankAccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (3, 6.5, 100, 'Term Basic')");
            Sql("INSERT INTO dbo.BankAccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (1, 0, 25, 'Checking Basic')");

            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee) VALUES (1, 0, 1)");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee) VALUES (2, 1, 1)");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee) VALUES (3, 2, 2)");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee) VALUES (4, 3, 25)");
            Sql("INSERT INTO dbo.TransactionTypes (id, Category, Fee) VALUES (5, 4, 5)");
        }
        
        public override void Down()
        {
        }
    }
}
