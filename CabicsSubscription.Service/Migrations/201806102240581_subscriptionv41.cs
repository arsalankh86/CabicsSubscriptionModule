namespace CabicsSubscription.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subscriptionv41 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DbErrorLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModuleName = c.String(unicode: false),
                        ModuleFunction = c.String(unicode: false),
                        FunctionArgument = c.String(unicode: false),
                        Error = c.String(unicode: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DbErrorLogs");
        }
    }
}
