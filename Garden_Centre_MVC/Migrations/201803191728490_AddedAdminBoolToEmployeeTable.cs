namespace Garden_Centre_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAdminBoolToEmployeeTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Admin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "Admin");
        }
    }
}
