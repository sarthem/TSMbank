namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequestClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IndividualId = c.String(maxLength: 128),
                        Status = c.Int(nullable: false),
                        SubmissionDate = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        BankAccNumber = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        BankAccount_AccountNumber = c.String(maxLength: 16),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.IndividualId)
                .ForeignKey("dbo.BankAccounts", t => t.BankAccount_AccountNumber)
                .Index(t => t.IndividualId)
                .Index(t => t.BankAccount_AccountNumber);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "BankAccount_AccountNumber", "dbo.BankAccounts");
            DropForeignKey("dbo.Requests", "IndividualId", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "BankAccount_AccountNumber" });
            DropIndex("dbo.Requests", new[] { "IndividualId" });
            DropTable("dbo.Requests");
        }
    }
}
