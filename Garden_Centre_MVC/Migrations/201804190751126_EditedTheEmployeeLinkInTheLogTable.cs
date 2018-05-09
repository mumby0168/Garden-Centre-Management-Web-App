namespace Garden_Centre_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditedTheEmployeeLinkInTheLogTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Logs", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Logs", new[] { "EmployeeId" });
            AddColumn("dbo.Logs", "EmployeeLoginId", c => c.Int(nullable: false));
            CreateIndex("dbo.Logs", "EmployeeLoginId");
            AddForeignKey("dbo.Logs", "EmployeeLoginId", "dbo.EmployeeLogins", "EmployeeLoginId", cascadeDelete: true);
            DropColumn("dbo.Logs", "EmployeeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Logs", "EmployeeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Logs", "EmployeeLoginId", "dbo.EmployeeLogins");
            DropIndex("dbo.Logs", new[] { "EmployeeLoginId" });
            DropColumn("dbo.Logs", "EmployeeLoginId");
            CreateIndex("dbo.Logs", "EmployeeId");
            AddForeignKey("dbo.Logs", "EmployeeId", "dbo.Employees", "EmployeeId", cascadeDelete: true);
        }
    }
}
