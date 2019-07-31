namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SeedDbWithTsmAcc : DbMigration
    {
        public override void Up()
        {
            //Sql(@"SET IDENTITY_INSERT [dbo].[Addresses] ON 
            //    INSERT [dbo].[Addresses] ([Id], [Country], [City], [Street], [StreetNumber], [PostalCode], [Region]) VALUES (104, N'Greece', N'Athens', N'TSMHQ', N'100', N'12345', N'TSMHQ')
            //    SET IDENTITY_INSERT [dbo].[Addresses] OFF
            //    INSERT [dbo].[AspNetUsers] ([Id], [RegisterCompletion], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'3g81b2cc-2659-49cc-9537-1b25d2273123', 1, N'info@tsmbank.com', 1, N'APc9pyECrIg4LhhzLec8D/LQ/r+T0Wg3c7C/w0v+1lkbQVB55YPUOamHmAObn2jj7A==', N'fd0a2c01-09b0-4827-a256-f4f12aa6d6fd', NULL, 0, 0, NULL, 1, 0, N'info@tsmbank.com')
            //    INSERT [dbo].[Individuals] ([Id], [FirstName], [LastName], [FathersName], [Email], [DateOfBirth], [IdentificationCardNo], [SSN], [VatNumber], [CreatedDate], [Status], [PrimaryAddressId], [SecondaryAddressId]) VALUES (N'3g81b2cc-2659-49cc-9537-1b25d2273123', N'-', N'TSMBANK', N'-', N'info@tsmbank.com', CAST(N'2000-01-01T00:00:00.000' AS DateTime), N'AA000000', N'00000000000', N'000000000', CAST(N'2019-07-30T13:09:05.287' AS DateTime), 1, 104, NULL)
            //    INSERT [dbo].[BankAccounts] ([AccountNumber], [AccountStatus], [Balance], [WithdrawalLimit], [NickName], [OpenedDate], [StatusUpdatedDateTime], [IndividualId], [BankAccountTypeId]) VALUES (N'1111222233334444', 0, CAST(0.00 AS Decimal(18, 2)), CAST(99999999999.99 AS Decimal(18, 2)), N'Tsm Bank Repository', CAST(N'2019-07-28T17:31:43.193' AS DateTime), CAST(N'2019-07-28T17:31:43.193' AS DateTime), N'3g81b2cc-2659-49cc-9537-1b25d2273123', 22)
            //    SET IDENTITY_INSERT [dbo].[Phones] ON 

            //    INSERT [dbo].[Phones] ([Id], [CountryCode], [PhoneNumber], [PhoneType], [IndividualId]) VALUES (104, N'0030', N'2101234567', 2, N'3g81b2cc-2659-49cc-9537-1b25d2273123')
            //    SET IDENTITY_INSERT [dbo].[Phones] OFF
            //    INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'3g81b2cc-2659-49cc-9537-1b25d2273123', N'69001b31-1271-40cf-9ad0-9b1e382a7138')");
        }

        public override void Down()
        {
        }
    }
}
