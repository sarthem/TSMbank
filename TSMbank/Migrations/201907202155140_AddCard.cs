namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 16),
                        CardHolderName = c.String(),
                        Brand = c.String(),
                        IssueDate = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        CVV = c.String(),
                        TransactionAmountLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Number = c.String(nullable: false, maxLength: 16),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccounts", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.Number, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cards", "Id", "dbo.BankAccounts");
            DropIndex("dbo.Cards", new[] { "Number" });
            DropIndex("dbo.Cards", new[] { "Id" });
            DropTable("dbo.Cards");
        }
    }
}
