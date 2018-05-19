namespace CabicsSubscription.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subscriptionv2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RefundTranactionLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubscriptionId = c.Int(nullable: false),
                        Message = c.String(unicode: false),
                        RefundTransactionId = c.String(unicode: false),
                        Gateway = c.String(unicode: false),
                        TransactionId = c.String(unicode: false),
                        CreatedDateTime = c.DateTime(nullable: false, precision: 0),
                        IsActive = c.Boolean(nullable: false),
                        Errors = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RefundTranactionLogs");
        }
    }
}
