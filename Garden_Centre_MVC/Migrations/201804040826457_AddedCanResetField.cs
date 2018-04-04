namespace Garden_Centre_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCanResetField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeLogins", "CanReset", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmployeeLogins", "CanReset");
        }
    }
}
