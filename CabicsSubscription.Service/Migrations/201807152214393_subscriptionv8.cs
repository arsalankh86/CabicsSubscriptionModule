namespace CabicsSubscription.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subscriptionv8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Password", c => c.String(unicode: false));
            AddColumn("dbo.Accounts", "CabicsSystemId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "CabicsSystemId");
            DropColumn("dbo.Accounts", "Password");
        }
    }
}
