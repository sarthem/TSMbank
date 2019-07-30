namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class populateAccounts : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT [dbo].[AspNetUsers] ([Id], [RegisterCompletion], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'4a29bd7c-dcee-4633-8e9c-603924cc8217', 0, N'admin@tsmbank.com', 1, N'AMmh1QFS3MYk0lwqoC17TJ2asNcoRp5V5GNQCMQlnMoz+LnzCfHTNIz/r7/2rr54jw==', N'1028c078-7809-46a7-b5e8-9c0250beab68', NULL, 0, 0, NULL, 1, 0, N'admin@tsmbank.com')
                INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'eef669aa-f90e-46d1-9b71-9508c1ddc97b', N'Administrator')
                INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'69001b31-1271-40cf-9ad0-9b1e382a7138', N'Customer')
                INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'4a29bd7c-dcee-4633-8e9c-603924cc8217', N'eef669aa-f90e-46d1-9b71-9508c1ddc97b')
                SET IDENTITY_INSERT [dbo].[Addresses] ON 
                INSERT [dbo].[Addresses] ([Id], [Country], [City], [Street], [StreetNumber], [PostalCode], [Region]) VALUES (101, N'Greece', N'Athens', N'Kifisias', N'55', N'56456', N'Marousi')
                INSERT [dbo].[Addresses] ([Id], [Country], [City], [Street], [StreetNumber], [PostalCode], [Region]) VALUES (102, N'Greece', N'Athens', N'Anthewn', N'11', N'14545', N'Athens')
                INSERT [dbo].[Addresses] ([Id], [Country], [City], [Street], [StreetNumber], [PostalCode], [Region]) VALUES (103, N'Greece', N'Athens', N'3hs Septemvriou', N'156', N'56897', N'Athens')
                SET IDENTITY_INSERT [dbo].[Addresses] OFF
                INSERT [dbo].[AspNetUsers] ([Id], [RegisterCompletion], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'3f71b28b-2659-49b2-9537-9a25d2273904', 1, N'eydap@emailna.co', 1, N'APc9pyECrIg4LhhzLec8D/LQ/r+T0Wg3c7C/w0v+1lkbQVB55YPUOamHmAObn2jj7A==', N'fd0a2c01-09b0-4827-a256-f4f12aa6d6fd', NULL, 0, 0, NULL, 1, 0, N'eydap@emailna.co')
                INSERT [dbo].[AspNetUsers] ([Id], [RegisterCompletion], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'6cdd582e-e470-4042-8248-9d8426c31409', 1, N'dei@emailna.co', 1, N'AO/uclDPUiPmJkbM1YfM5HbTjRr0/ZVxpTzm0kGhZiaKCO/iynhGRZY59Whku7zujQ==', N'58208ec3-d9c2-4177-9c23-23086fa985b7', NULL, 0, 0, NULL, 1, 0, N'dei@emailna.co')
                INSERT [dbo].[AspNetUsers] ([Id], [RegisterCompletion], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'a58c9db4-7525-4b82-98df-555f980a5956', 1, N'ote@emailna.co', 1, N'AKoRprdBVpX0ZHKm7h5So7Ih4iO91muvxR1nC933OdHivBjpiQg3ubaKLIroa5sODg==', N'a02b7b22-e0c7-4d85-94c5-b8b3977e7987', NULL, 0, 0, NULL, 1, 0, N'ote@emailna.co')
                INSERT [dbo].[Individuals] ([Id], [FirstName], [LastName], [FathersName], [Email], [DateOfBirth], [IdentificationCardNo], [SSN], [VatNumber], [CreatedDate], [Status], [PrimaryAddressId], [SecondaryAddressId]) VALUES (N'3f71b28b-2659-49b2-9537-9a25d2273904', N'-', N'EYDAP', N'-', N'eydap@emailna.co', CAST(N'2000-01-01T00:00:00.000' AS DateTime), N'AA000000', N'00000000000', N'000000000', CAST(N'2019-07-30T13:17:41.143' AS DateTime), 1, 102, NULL)
                INSERT [dbo].[Individuals] ([Id], [FirstName], [LastName], [FathersName], [Email], [DateOfBirth], [IdentificationCardNo], [SSN], [VatNumber], [CreatedDate], [Status], [PrimaryAddressId], [SecondaryAddressId]) VALUES (N'6cdd582e-e470-4042-8248-9d8426c31409', N'-', N'DEH', N'-', N'dei@emailna.co', CAST(N'2000-01-01T00:00:00.000' AS DateTime), N'AA000000', N'00000000000', N'000000000', CAST(N'2019-07-30T13:21:05.027' AS DateTime), 1, 103, NULL)
                INSERT [dbo].[Individuals] ([Id], [FirstName], [LastName], [FathersName], [Email], [DateOfBirth], [IdentificationCardNo], [SSN], [VatNumber], [CreatedDate], [Status], [PrimaryAddressId], [SecondaryAddressId]) VALUES (N'a58c9db4-7525-4b82-98df-555f980a5956', N'-', N'OTE', N'-', N'ote@emailna.co', CAST(N'2000-01-01T00:00:00.000' AS DateTime), N'AA000000', N'00000000000', N'000000000', CAST(N'2019-07-30T13:09:05.287' AS DateTime), 1, 101, NULL)
                INSERT [dbo].[BankAccounts] ([AccountNumber], [AccountStatus], [Balance], [WithdrawalLimit], [NickName], [OpenedDate], [StatusUpdatedDateTime], [IndividualId], [BankAccountTypeId]) VALUES (N'1111222233334441', 0, CAST(0.00 AS Decimal(18, 2)), CAST(99999999999.99 AS Decimal(18, 2)), N'EYDAP', CAST(N'2019-07-28T16:05:33.747' AS DateTime), CAST(N'2019-07-28T16:05:33.747' AS DateTime), N'3f71b28b-2659-49b2-9537-9a25d2273904', 51)
                INSERT [dbo].[BankAccounts] ([AccountNumber], [AccountStatus], [Balance], [WithdrawalLimit], [NickName], [OpenedDate], [StatusUpdatedDateTime], [IndividualId], [BankAccountTypeId]) VALUES (N'1111222233334442', 0, CAST(0.00 AS Decimal(18, 2)), CAST(99999999999.99 AS Decimal(18, 2)), N'DEH', CAST(N'2019-07-28T16:46:46.780' AS DateTime), CAST(N'2019-07-28T16:46:46.780' AS DateTime), N'6cdd582e-e470-4042-8248-9d8426c31409', 51)
                INSERT [dbo].[BankAccounts] ([AccountNumber], [AccountStatus], [Balance], [WithdrawalLimit], [NickName], [OpenedDate], [StatusUpdatedDateTime], [IndividualId], [BankAccountTypeId]) VALUES (N'1111222233334443', 0, CAST(0.00 AS Decimal(18, 2)), CAST(99999999999.99 AS Decimal(18, 2)), N'OTE', CAST(N'2019-07-28T17:31:43.193' AS DateTime), CAST(N'2019-07-28T17:31:43.193' AS DateTime), N'a58c9db4-7525-4b82-98df-555f980a5956', 51)
                SET IDENTITY_INSERT [dbo].[Phones] ON 
                INSERT [dbo].[Phones] ([Id], [CountryCode], [PhoneNumber], [PhoneType], [IndividualId]) VALUES (101, N'0030', N'2106656478', 2, N'a58c9db4-7525-4b82-98df-555f980a5956')
                INSERT [dbo].[Phones] ([Id], [CountryCode], [PhoneNumber], [PhoneType], [IndividualId]) VALUES (102, N'0030', N'2103325568', 2, N'3f71b28b-2659-49b2-9537-9a25d2273904')
                INSERT [dbo].[Phones] ([Id], [CountryCode], [PhoneNumber], [PhoneType], [IndividualId]) VALUES (103, N'0030', N'2105645789', 2, N'6cdd582e-e470-4042-8248-9d8426c31409')
                SET IDENTITY_INSERT [dbo].[Phones] OFF
                INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'3f71b28b-2659-49b2-9537-9a25d2273904', N'69001b31-1271-40cf-9ad0-9b1e382a7138')
                INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'6cdd582e-e470-4042-8248-9d8426c31409', N'69001b31-1271-40cf-9ad0-9b1e382a7138')
                INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'a58c9db4-7525-4b82-98df-555f980a5956', N'69001b31-1271-40cf-9ad0-9b1e382a7138')");
        }
        
        public override void Down()
        {
        }
    }
}
