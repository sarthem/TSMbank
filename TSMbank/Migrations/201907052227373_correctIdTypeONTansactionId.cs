namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correctIdTypeONTansactionId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transactions", "TypeId", "dbo.TransactionTypes");
            DropIndex("dbo.Transactions", new[] { "TypeId" });
            DropPrimaryKey("dbo.TransactionTypes");
            AlterColumn("dbo.Transactions", "TypeId", c => c.Byte(nullable: false));
            AlterColumn("dbo.TransactionTypes", "Id", c => c.Byte(nullable: false));
            AddPrimaryKey("dbo.TransactionTypes", "Id");
            CreateIndex("dbo.Transactions", "TypeId");
            AddForeignKey("dbo.Transactions", "TypeId", "dbo.TransactionTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "TypeId", "dbo.TransactionTypes");
            DropIndex("dbo.Transactions", new[] { "TypeId" });
            DropPrimaryKey("dbo.TransactionTypes");
            AlterColumn("dbo.TransactionTypes", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Transactions", "TypeId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.TransactionTypes", "Id");
            CreateIndex("dbo.Transactions", "TypeId");
            AddForeignKey("dbo.Transactions", "TypeId", "dbo.TransactionTypes", "Id", cascadeDelete: true);
        }
    }
}
