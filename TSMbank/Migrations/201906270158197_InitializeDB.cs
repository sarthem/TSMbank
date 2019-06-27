namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializeDB : DbMigration
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
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        TypeOfTransaction = c.Int(nullable: false),
                        TransactionDateAndTime = c.DateTime(nullable: false),
                        CredidAccountNo = c.String(),
                        CreditIBAN = c.String(),
                        CreditAccountBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditAccountCurrency = c.String(),
                        CreditAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditAccountBalanceAfterTransaction = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DebitAccountNo = c.String(),
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
                        CreditAccount_AccountNumber = c.String(maxLength: 16),
                        DebitAccount_AccountNumber = c.String(maxLength: 16),
                        Account_AccountNumber = c.String(maxLength: 16),
                        Account_AccountNumber1 = c.String(maxLength: 16),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Accounts", t => t.CreditAccount_AccountNumber)
                .ForeignKey("dbo.Accounts", t => t.DebitAccount_AccountNumber)
                .ForeignKey("dbo.Accounts", t => t.Account_AccountNumber)
                .ForeignKey("dbo.Accounts", t => t.Account_AccountNumber1)
                .Index(t => t.CreditAccount_AccountNumber)
                .Index(t => t.DebitAccount_AccountNumber)
                .Index(t => t.Account_AccountNumber)
                .Index(t => t.Account_AccountNumber1);
            
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
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 255),
                        LastName = c.String(nullable: false, maxLength: 255),
                        FatherName = c.String(nullable: false, maxLength: 255),
                        Email = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        IdentificationCardNo = c.String(),
                        SSN = c.String(),
                        VatNumber = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        IsActive = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        PhoneId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: true)
                .ForeignKey("dbo.Phones", t => t.PhoneId, cascadeDelete: true)
                .Index(t => t.AddressId)
                .Index(t => t.PhoneId);
            
            CreateTable(
                "dbo.Phones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryCode = c.String(),
                        PhoneNumber = c.String(),
                        PhoneType = c.Int(nullable: false),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
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
            DropForeignKey("dbo.Phones", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Customers", "PhoneId", "dbo.Phones");
            DropForeignKey("dbo.Addresses", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Customers", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Accounts", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Transactions", "Account_AccountNumber1", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "Account_AccountNumber", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "DebitAccount_AccountNumber", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "CreditAccount_AccountNumber", "dbo.Accounts");
            DropForeignKey("dbo.Accounts", "AccountType_Id", "dbo.AccountTypes");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Phones", new[] { "Customer_Id" });
            DropIndex("dbo.Customers", new[] { "PhoneId" });
            DropIndex("dbo.Customers", new[] { "AddressId" });
            DropIndex("dbo.Addresses", new[] { "Customer_Id" });
            DropIndex("dbo.Transactions", new[] { "Account_AccountNumber1" });
            DropIndex("dbo.Transactions", new[] { "Account_AccountNumber" });
            DropIndex("dbo.Transactions", new[] { "DebitAccount_AccountNumber" });
            DropIndex("dbo.Transactions", new[] { "CreditAccount_AccountNumber" });
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
