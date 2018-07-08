namespace CabicsSubscription.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Plans", "PlanExpiryDateString", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Plans", "PlanExpiryDateString");
        }
    }
}
