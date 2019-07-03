namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAccountStatusUpdatedDateTimeToNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accounts", "StatusUpdatedDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accounts", "StatusUpdatedDateTime", c => c.DateTime(nullable: false));
        }
    }
}
