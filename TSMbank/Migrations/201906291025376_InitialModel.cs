namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountNumber = c.String(nullable: false, maxLength: 16),
                        AccountStatus = c.Int(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WithdrawalLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NickName = c.String(),
                        OpenedDate = c.DateTime(nullable: false),
                        StatusUpdatedDateTime = c.DateTime(nullable: false),
                        AccountTypeId = c.Byte(nullable: false),
                        AccountType_Id = c.Int(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.AccountNumber)
                .ForeignKey("dbo.AccountTypes", t => t.AccountType_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.AccountType_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.AccountTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.Int(nullable: false),
                        InterestRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PeriodicFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Summary = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        ValueDateTime = c.DateTime(nullable: false),
                        CreditAccountNo = c.String(nullable: false, maxLength: 16),
                        CreditIBAN = c.String(),
                        CreditAccountBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditAccountCurrency = c.String(),
                        CreditAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditAccountBalanceAfterTransaction = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DebitAccountNo = c.String(nullable: false, maxLength: 16),
                        DebitIBAN = c.String(),
                        DebitAccountBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DebitAccountCurrency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DebitAmount = c.Int(nullable: false),
                        DebitAccountBalanceAfterTransaction = c.Int(nullable: false),
                        BankFee = c.Int(nullable: false),
                        ApprovedFromBankManager = c.Boolean(nullable: false),
                        PendingForApproval = c.Boolean(nullable: false),
                        TransactionApprovedReview = c.Int(nullable: false),
                        IsCompleted = c.Boolean(nullable: false),
                        CancelledTransactionId = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Transactions", t => t.CancelledTransactionId)
                .ForeignKey("dbo.Accounts", t => t.CreditAccountNo)
                .ForeignKey("dbo.Accounts", t => t.DebitAccountNo)
                .Index(t => t.CreditAccountNo)
                .Index(t => t.DebitAccountNo)
                .Index(t => t.CancelledTransactionId);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Country = c.String(),
                        City = c.String(),
                        Street = c.String(),
                        StreetNumber = c.String(),
                        PostalCode = c.String(),
                        Region = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 255),
                        LastName = c.String(nullable: false, maxLength: 255),
                        FathersName = c.String(nullable: false, maxLength: 255),
                        Email = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        IdentificationCardNo = c.String(),
                        SSN = c.String(),
                        VatNumber = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        PrimaryAddressId = c.Int(nullable: false),
                        SecondaryAddressId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.PrimaryAddressId)
                .ForeignKey("dbo.Addresses", t => t.SecondaryAddressId)
                .Index(t => t.PrimaryAddressId)
                .Index(t => t.SecondaryAddressId);
            
            CreateTable(
                "dbo.Phones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryCode = c.String(),
                        PhoneNumber = c.String(),
                        PhoneType = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Customers", "SecondaryAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Customers", "PrimaryAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Phones", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Accounts", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Transactions", "DebitAccountNo", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "CreditAccountNo", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "CancelledTransactionId", "dbo.Transactions");
            DropForeignKey("dbo.Accounts", "AccountType_Id", "dbo.AccountTypes");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Phones", new[] { "CustomerId" });
            DropIndex("dbo.Customers", new[] { "SecondaryAddressId" });
            DropIndex("dbo.Customers", new[] { "PrimaryAddressId" });
            DropIndex("dbo.Transactions", new[] { "CancelledTransactionId" });
            DropIndex("dbo.Transactions", new[] { "DebitAccountNo" });
            DropIndex("dbo.Transactions", new[] { "CreditAccountNo" });
            DropIndex("dbo.Accounts", new[] { "Customer_Id" });
            DropIndex("dbo.Accounts", new[] { "AccountType_Id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Phones");
            DropTable("dbo.Customers");
            DropTable("dbo.Addresses");
            DropTable("dbo.Transactions");
            DropTable("dbo.AccountTypes");
            DropTable("dbo.Accounts");
        }
    }
}
