namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CardRequestMods : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "CardType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "CardType");
        }
    }
}
