namespace CabicsSubscription.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subscriptionv3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TextlocalConfigurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CabofficeId = c.Int(nullable: false),
                        Username = c.String(unicode: false),
                        Password = c.String(unicode: false),
                        APIKey = c.String(unicode: false),
                        Hash = c.String(unicode: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TextlocalConfigurations");
        }
    }
}
