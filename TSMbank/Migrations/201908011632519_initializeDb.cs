namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initializeDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Country = c.String(nullable: false, maxLength: 50),
                        City = c.String(nullable: false, maxLength: 50),
                        Street = c.String(nullable: false, maxLength: 255),
                        StreetNumber = c.String(nullable: false, maxLength: 9),
                        PostalCode = c.String(nullable: false, maxLength: 5),
                        Region = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        AccountNumber = c.String(nullable: false, maxLength: 16),
                        AccountStatus = c.Int(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WithdrawalLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NickName = c.String(),
                        OpenedDate = c.DateTime(nullable: false),
                        StatusUpdatedDateTime = c.DateTime(),
                        IndividualId = c.String(maxLength: 128),
                        BankAccountTypeId = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.AccountNumber)
                .ForeignKey("dbo.BankAccountTypes", t => t.BankAccountTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Individuals", t => t.IndividualId)
                .Index(t => t.IndividualId)
                .Index(t => t.BankAccountTypeId);
            
            CreateTable(
                "dbo.BankAccountTypes",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Description = c.Int(nullable: false),
                        InterestRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PeriodicFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Summary = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        TypeId = c.Byte(nullable: false),
                        ValueDateTime = c.DateTime(nullable: false),
                        DebitAccountNo = c.String(nullable: false, maxLength: 16),
                        DebitIBAN = c.String(),
                        DebitAccountBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DebitAccountCurrency = c.String(),
                        DebitAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DebitAccountBalanceAfterTransaction = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditAccountNo = c.String(nullable: false, maxLength: 16),
                        CreditIBAN = c.String(),
                        CreditAccountBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditAccountCurrency = c.String(),
                        CreditAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditAccountBalanceAfterTransaction = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsCompleted = c.Boolean(nullable: false),
                        CancelledTransactionId = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Transactions", t => t.CancelledTransactionId)
                .ForeignKey("dbo.TransactionTypes", t => t.TypeId, cascadeDelete: true)
                .ForeignKey("dbo.BankAccounts", t => t.CreditAccountNo)
                .ForeignKey("dbo.BankAccounts", t => t.DebitAccountNo)
                .Index(t => t.TypeId)
                .Index(t => t.DebitAccountNo)
                .Index(t => t.CreditAccountNo)
                .Index(t => t.CancelledTransactionId);
            
            CreateTable(
                "dbo.TransactionTypes",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Category = c.Int(nullable: false),
                        Fee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Individuals",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 255),
                        LastName = c.String(nullable: false, maxLength: 255),
                        FathersName = c.String(nullable: false, maxLength: 255),
                        Email = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        IdentificationCardNo = c.String(nullable: false, maxLength: 8),
                        SSN = c.String(nullable: false, maxLength: 11),
                        VatNumber = c.String(nullable: false, maxLength: 9),
                        CreatedDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        PrimaryAddressId = c.Int(nullable: false),
                        SecondaryAddressId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.PrimaryAddressId)
                .ForeignKey("dbo.Addresses", t => t.SecondaryAddressId)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.PrimaryAddressId)
                .Index(t => t.SecondaryAddressId);
            
            CreateTable(
                "dbo.Phones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryCode = c.String(nullable: false, maxLength: 4),
                        PhoneNumber = c.String(nullable: false, maxLength: 10),
                        PhoneType = c.Int(nullable: false),
                        IndividualId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Individuals", t => t.IndividualId)
                .Index(t => t.IndividualId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RegisterCompletion = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IndividualId = c.String(maxLength: 128),
                        Status = c.Int(nullable: false),
                        SubmissionDate = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        BankAccTypeId = c.Byte(),
                        CreditLimit = c.Decimal(precision: 18, scale: 2),
                        TransactionAmountLimit = c.Decimal(precision: 18, scale: 2),
                        CardType = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccountTypes", t => t.BankAccTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Individuals", t => t.IndividualId)
                .Index(t => t.IndividualId)
                .Index(t => t.BankAccTypeId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Requests", "IndividualId", "dbo.Individuals");
            DropForeignKey("dbo.Requests", "BankAccTypeId", "dbo.BankAccountTypes");
            DropForeignKey("dbo.Individuals", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Individuals", "SecondaryAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Individuals", "PrimaryAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Phones", "IndividualId", "dbo.Individuals");
            DropForeignKey("dbo.BankAccounts", "IndividualId", "dbo.Individuals");
            DropForeignKey("dbo.Transactions", "DebitAccountNo", "dbo.BankAccounts");
            DropForeignKey("dbo.Transactions", "CreditAccountNo", "dbo.BankAccounts");
            DropForeignKey("dbo.Transactions", "TypeId", "dbo.TransactionTypes");
            DropForeignKey("dbo.Transactions", "CancelledTransactionId", "dbo.Transactions");
            DropForeignKey("dbo.Cards", "Id", "dbo.BankAccounts");
            DropForeignKey("dbo.BankAccounts", "BankAccountTypeId", "dbo.BankAccountTypes");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Requests", new[] { "BankAccTypeId" });
            DropIndex("dbo.Requests", new[] { "IndividualId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Phones", new[] { "IndividualId" });
            DropIndex("dbo.Individuals", new[] { "SecondaryAddressId" });
            DropIndex("dbo.Individuals", new[] { "PrimaryAddressId" });
            DropIndex("dbo.Individuals", new[] { "Id" });
            DropIndex("dbo.Transactions", new[] { "CancelledTransactionId" });
            DropIndex("dbo.Transactions", new[] { "CreditAccountNo" });
            DropIndex("dbo.Transactions", new[] { "DebitAccountNo" });
            DropIndex("dbo.Transactions", new[] { "TypeId" });
            DropIndex("dbo.Cards", new[] { "Number" });
            DropIndex("dbo.Cards", new[] { "Id" });
            DropIndex("dbo.BankAccounts", new[] { "BankAccountTypeId" });
            DropIndex("dbo.BankAccounts", new[] { "IndividualId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Requests");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Phones");
            DropTable("dbo.Individuals");
            DropTable("dbo.TransactionTypes");
            DropTable("dbo.Transactions");
            DropTable("dbo.Cards");
            DropTable("dbo.BankAccountTypes");
            DropTable("dbo.BankAccounts");
            DropTable("dbo.Addresses");
        }
    }
}
