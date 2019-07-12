namespace TSMbank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionToTransactionType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransactionTypes", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransactionTypes", "Description");
        }
    }
}
