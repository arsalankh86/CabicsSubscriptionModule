namespace CabicsSubscription.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrationv41 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "BtCustomerId", c => c.String(unicode: false));
            AddColumn("dbo.Accounts", "BtPaymentMethodToken", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "BtPaymentMethodToken");
            DropColumn("dbo.Accounts", "BtCustomerId");
        }
    }
}
