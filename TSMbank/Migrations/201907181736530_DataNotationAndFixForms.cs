namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataNotationAndFixForms : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Addresses", "Country", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Addresses", "City", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Addresses", "Street", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Addresses", "StreetNumber", c => c.String(nullable: false, maxLength: 9));
            AlterColumn("dbo.Addresses", "PostalCode", c => c.String(nullable: false, maxLength: 5));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Addresses", "PostalCode", c => c.String(maxLength: 5));
            AlterColumn("dbo.Addresses", "StreetNumber", c => c.String(maxLength: 9));
            AlterColumn("dbo.Addresses", "Street", c => c.String(maxLength: 255));
            AlterColumn("dbo.Addresses", "City", c => c.String(maxLength: 50));
            AlterColumn("dbo.Addresses", "Country", c => c.String(maxLength: 50));
        }
    }
}
