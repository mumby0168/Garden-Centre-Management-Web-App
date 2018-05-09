namespace Garden_Centre_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTheLogAndActionTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        LogId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        ActionTypeId = c.Int(nullable: false),
                        PropertyEffected = c.String(),
                        DateOfAction = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LogId)
                .ForeignKey("dbo.ActionTypes", t => t.ActionTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.ActionTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Logs", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Logs", "ActionTypeId", "dbo.ActionTypes");
            DropIndex("dbo.Logs", new[] { "ActionTypeId" });
            DropIndex("dbo.Logs", new[] { "EmployeeId" });
            DropTable("dbo.Logs");
            DropTable("dbo.ActionTypes");
        }
    }
}
