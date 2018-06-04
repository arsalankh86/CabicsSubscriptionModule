namespace CabicsSubscription.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subscriptionv31 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WindowsServiceExecutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WindowsServiceFunctionCode = c.Int(nullable: false),
                        WindowsServiceFunction = c.String(unicode: false),
                        WindowsServiceArgumrnt = c.Int(nullable: false),
                        WindowsServiceStatus = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        UpdatedDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Accounts", "PaymentMethodNonce", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "PaymentMethodNonce");
            DropTable("dbo.WindowsServiceExecutions");
        }
    }
}
