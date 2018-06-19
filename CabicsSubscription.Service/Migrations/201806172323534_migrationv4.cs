namespace CabicsSubscription.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrationv4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CreditDeductionLogs", "CreatedDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.CreditDeductionLogs", "UpdatedDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.CreditDeductionLogs", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CreditDeductionLogs", "IsActive");
            DropColumn("dbo.CreditDeductionLogs", "UpdatedDate");
            DropColumn("dbo.CreditDeductionLogs", "CreatedDate");
        }
    }
}
