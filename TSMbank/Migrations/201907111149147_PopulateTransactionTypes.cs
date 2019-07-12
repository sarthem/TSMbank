namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class PopulateTransactionTypes : DbMigration
    {
        public override void Up()
        {
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
