namespace Garden_Centre_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SwappedEmployeeRelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "EmployeeLoginId", "dbo.EmployeeLogins");
            DropIndex("dbo.Employees", new[] { "EmployeeLoginId" });
            AddColumn("dbo.EmployeeLogins", "EmployeeId", c => c.Int(nullable: false));
            CreateIndex("dbo.EmployeeLogins", "EmployeeId");
            AddForeignKey("dbo.EmployeeLogins", "EmployeeId", "dbo.Employees", "EmployeeId", cascadeDelete: true);
            DropColumn("dbo.Employees", "EmployeeLoginId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "EmployeeLoginId", c => c.Int(nullable: false));
            DropForeignKey("dbo.EmployeeLogins", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.EmployeeLogins", new[] { "EmployeeId" });
            DropColumn("dbo.EmployeeLogins", "EmployeeId");
            CreateIndex("dbo.Employees", "EmployeeLoginId");
            AddForeignKey("dbo.Employees", "EmployeeLoginId", "dbo.EmployeeLogins", "EmployeeLoginId", cascadeDelete: true);
        }
    }
}
