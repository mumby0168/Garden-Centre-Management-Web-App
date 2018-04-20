namespace Garden_Centre_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAccountCreatedFlagToEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "AccountCreated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "AccountCreated");
        }
    }
}
