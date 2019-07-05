namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBoolAAplicationuserOnregister : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RegisterCompletion", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "RegisterCompletion");
        }
    }
}
