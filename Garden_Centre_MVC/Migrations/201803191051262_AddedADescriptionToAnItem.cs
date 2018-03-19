namespace Garden_Centre_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedADescriptionToAnItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "Description");
        }
    }
}
