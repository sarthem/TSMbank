namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreditCardRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "CreditLimit", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Requests", "TransactionAmountLimit", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "TransactionAmountLimit");
            DropColumn("dbo.Requests", "CreditLimit");
        }
    }
}
