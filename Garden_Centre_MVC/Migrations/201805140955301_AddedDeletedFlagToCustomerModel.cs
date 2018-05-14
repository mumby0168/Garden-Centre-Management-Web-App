namespace Garden_Centre_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDeletedFlagToCustomerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "CustomerDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "CustomerDeleted");
        }
    }
}
