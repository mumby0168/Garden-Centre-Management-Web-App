namespace Garden_Centre_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 15),
                        SecondName = c.String(nullable: false, maxLength: 15),
                        AddressLine1 = c.String(nullable: false, maxLength: 25),
                        AddressLine2 = c.String(nullable: false, maxLength: 15),
                        PostCode = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionNumber = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Value = c.Single(nullable: false),
                        ItemId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        ItemPrice = c.Single(nullable: false),
                        Stock = c.Int(nullable: false),
                        OnOrder = c.Int(nullable: false),
                        Sold = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId);
            
            CreateTable(
                "dbo.EmployeeLogins",
                c => new
                    {
                        EmployeeLoginId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.Binary(),
                        Salt = c.Binary(),
                    })
                .PrimaryKey(t => t.EmployeeLoginId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 15),
                        SecondName = c.String(nullable: false, maxLength: 15),
                        EmployeeNumber = c.Int(nullable: false),
                        EmployeeLoginId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("dbo.EmployeeLogins", t => t.EmployeeLoginId, cascadeDelete: true)
                .Index(t => t.EmployeeLoginId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "EmployeeLoginId", "dbo.EmployeeLogins");
            DropForeignKey("dbo.Transactions", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Transactions", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Employees", new[] { "EmployeeLoginId" });
            DropIndex("dbo.Transactions", new[] { "CustomerId" });
            DropIndex("dbo.Transactions", new[] { "ItemId" });
            DropTable("dbo.Employees");
            DropTable("dbo.EmployeeLogins");
            DropTable("dbo.Items");
            DropTable("dbo.Transactions");
            DropTable("dbo.Customers");
        }
    }
}
