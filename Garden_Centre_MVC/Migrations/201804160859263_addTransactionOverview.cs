namespace Garden_Centre_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTransactionOverview : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransactionOverviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionNumber = c.Int(nullable: false),
                        TotalValue = c.Single(nullable: false),
                        Date = c.DateTime(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransactionOverviews", "CustomerId", "dbo.Customers");
            DropIndex("dbo.TransactionOverviews", new[] { "CustomerId" });
            DropTable("dbo.TransactionOverviews");
        }
    }
}
