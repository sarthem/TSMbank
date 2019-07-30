namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSgnalR : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Individuals", "DateOfBirth", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Individuals", "DateOfBirth", c => c.DateTime(nullable: false));
        }
    }
}
