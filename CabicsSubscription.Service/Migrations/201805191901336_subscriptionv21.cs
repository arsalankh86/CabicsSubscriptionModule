namespace CabicsSubscription.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subscriptionv21 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TranactionLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubscriptionId = c.Int(nullable: false),
                        TransactionId = c.String(unicode: false),
                        Gateway = c.String(unicode: false),
                        CreatedDateTime = c.DateTime(nullable: false, precision: 0),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TranactionLogs");
        }
    }
}
