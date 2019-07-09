namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeClassNameFromAccountToBankAccountsANdtoControllers : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Accounts", newName: "BankAccounts");
            RenameTable(name: "dbo.AccountTypes", newName: "BankAccountTypes");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.BankAccountTypes", newName: "AccountTypes");
            RenameTable(name: "dbo.BankAccounts", newName: "Accounts");
        }
    }
}
