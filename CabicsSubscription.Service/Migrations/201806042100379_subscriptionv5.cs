namespace CabicsSubscription.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subscriptionv5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Plans", "BrainTreePlanName", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Plans", "BrainTreePlanName");
        }
    }
}
