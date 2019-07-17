namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePropOnBankAccRequest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "BankAccount_AccountNumber", "dbo.BankAccounts");
            DropIndex("dbo.Requests", new[] { "BankAccount_AccountNumber" });
            AddColumn("dbo.Requests", "BankAccTypeId", c => c.Int());
            AddColumn("dbo.Requests", "BankAccSummury", c => c.String());
            DropColumn("dbo.Requests", "BankAccNumber");
            DropColumn("dbo.Requests", "BankAccount_AccountNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "BankAccount_AccountNumber", c => c.String(maxLength: 16));
            AddColumn("dbo.Requests", "BankAccNumber", c => c.String());
            DropColumn("dbo.Requests", "BankAccSummury");
            DropColumn("dbo.Requests", "BankAccTypeId");
            CreateIndex("dbo.Requests", "BankAccount_AccountNumber");
            AddForeignKey("dbo.Requests", "BankAccount_AccountNumber", "dbo.BankAccounts", "AccountNumber");
        }
    }
}
