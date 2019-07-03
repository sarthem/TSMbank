namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerFkToAccount : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Accounts", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Accounts", new[] { "Customer_Id" });
            RenameColumn(table: "dbo.Accounts", name: "Customer_Id", newName: "CustomerId");
            AlterColumn("dbo.Accounts", "CustomerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Accounts", "CustomerId");
            AddForeignKey("dbo.Accounts", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Accounts", new[] { "CustomerId" });
            AlterColumn("dbo.Accounts", "CustomerId", c => c.Int());
            RenameColumn(table: "dbo.Accounts", name: "CustomerId", newName: "Customer_Id");
            CreateIndex("dbo.Accounts", "Customer_Id");
            AddForeignKey("dbo.Accounts", "Customer_Id", "dbo.Customers", "Id");
        }
    }
}
