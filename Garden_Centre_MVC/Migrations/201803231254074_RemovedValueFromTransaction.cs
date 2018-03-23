namespace Garden_Centre_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedValueFromTransaction : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Transactions", "Value");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "Value", c => c.Single(nullable: false));
        }
    }
}
