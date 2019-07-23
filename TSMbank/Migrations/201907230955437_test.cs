namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Requests", "BankAccTypeId");
            AddForeignKey("dbo.Requests", "BankAccTypeId", "dbo.BankAccountTypes", "Id", cascadeDelete: true);
            DropColumn("dbo.Requests", "BankAccSummury");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "BankAccSummury", c => c.String());
            DropForeignKey("dbo.Requests", "BankAccTypeId", "dbo.BankAccountTypes");
            DropIndex("dbo.Requests", new[] { "BankAccTypeId" });
        }
    }
}
