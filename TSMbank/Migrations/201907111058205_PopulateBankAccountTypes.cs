namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateBankAccountTypes : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.BankAccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (0, 0.5, 25, 'Checking Gold')");
            Sql("INSERT INTO dbo.BankAccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (1, 3.5, 50, 'Savings Basic')");
            Sql("INSERT INTO dbo.BankAccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (2, 6.5, 100, 'Term Basic')");
            Sql("INSERT INTO dbo.BankAccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (0, 0, 25, 'Checking Basic')");
            Sql("INSERT INTO dbo.BankAccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (3, 16, 30, 'Credit Card')");
            
        }
        
        public override void Down()
        {
        }
    }
}
