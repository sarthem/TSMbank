namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class PopulateAccountTypes : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.AccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (1, 0.5, 25, 'Checking Gold')");
            Sql("INSERT INTO dbo.AccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (2, 3.5, 50, 'Savings Basic')");
            Sql("INSERT INTO dbo.AccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (3, 6.5, 100, 'Term Basic')");
            Sql("INSERT INTO dbo.AccountTypes (Description, InterestRate, PeriodicFee, Summary) VALUES (1, 0, 25, 'Checking Basic')");
        }

        public override void Down()
        {
        }
    }
}
