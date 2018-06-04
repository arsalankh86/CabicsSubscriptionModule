namespace CabicsSubscription.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subscriptionv4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptions", "IsAutoRenewel", c => c.Boolean(nullable: false));
            AddColumn("dbo.Subscriptions", "NoOfBillingCycle", c => c.Int(nullable: false));
            AddColumn("dbo.Subscriptions", "btSubscriptionId", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscriptions", "btSubscriptionId");
            DropColumn("dbo.Subscriptions", "NoOfBillingCycle");
            DropColumn("dbo.Subscriptions", "IsAutoRenewel");
        }
    }
}
