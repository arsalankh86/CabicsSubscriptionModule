namespace CabicsSubscription.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subscription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Token", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "Token");
        }
    }
}
