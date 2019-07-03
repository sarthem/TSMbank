namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTransaction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransactionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category = c.Int(nullable: false),
                        Fee = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Transactions", "TypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Transactions", "DebitAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Transactions", "DebitAccountBalanceAfterTransaction", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.Transactions", "TypeId");
            AddForeignKey("dbo.Transactions", "TypeId", "dbo.TransactionTypes", "Id", cascadeDelete: true);
            DropColumn("dbo.Transactions", "Type");
            DropColumn("dbo.Transactions", "BankFee");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "BankFee", c => c.Int(nullable: false));
            AddColumn("dbo.Transactions", "Type", c => c.Int(nullable: false));
            DropForeignKey("dbo.Transactions", "TypeId", "dbo.TransactionTypes");
            DropIndex("dbo.Transactions", new[] { "TypeId" });
            AlterColumn("dbo.Transactions", "DebitAccountBalanceAfterTransaction", c => c.Int(nullable: false));
            AlterColumn("dbo.Transactions", "DebitAmount", c => c.Int(nullable: false));
            DropColumn("dbo.Transactions", "TypeId");
            DropTable("dbo.TransactionTypes");
        }
    }
}
