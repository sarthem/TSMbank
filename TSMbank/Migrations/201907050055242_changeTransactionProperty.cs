namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeTransactionProperty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transactions", "DebitAccountCurrency", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transactions", "DebitAccountCurrency", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
