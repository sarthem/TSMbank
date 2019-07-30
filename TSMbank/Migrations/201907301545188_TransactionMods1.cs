namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionMods1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Transactions", "ApprovedFromBankManager");
            DropColumn("dbo.Transactions", "PendingForApproval");
            DropColumn("dbo.Transactions", "TransactionApprovedReview");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "TransactionApprovedReview", c => c.Int(nullable: false));
            AddColumn("dbo.Transactions", "PendingForApproval", c => c.Boolean(nullable: false));
            AddColumn("dbo.Transactions", "ApprovedFromBankManager", c => c.Boolean(nullable: false));
        }
    }
}
