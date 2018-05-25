namespace CabicsSubscription.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subscriptionv22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Plans", "UpdatedDateTime", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Plans", "UpdatedDateTime");
        }
    }
}
