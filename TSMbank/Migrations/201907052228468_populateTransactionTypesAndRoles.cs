namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class populateTransactionTypesAndRoles : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.TransactionTypes (Category, Fee) VALUES (2, 1)");
            Sql("INSERT INTO dbo.TransactionTypes (Category, Fee) VALUES (3, 2)");
            Sql("INSERT INTO dbo.TransactionTypes (Category, Fee) VALUES (4, 25)");
            Sql("INSERT INTO dbo.TransactionTypes (Category, Fee) VALUES (5, 5)");
           
        }
        
        public override void Down()
        {
        }
    }
}
