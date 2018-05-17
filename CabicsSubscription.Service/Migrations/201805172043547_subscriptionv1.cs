namespace CabicsSubscription.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subscriptionv1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountCode = c.String(unicode: false),
                        FullName = c.String(unicode: false),
                        Email = c.String(unicode: false),
                        BalanceCredit = c.Int(),
                        AllCredit = c.Int(),
                        CurrentSubscriptionId = c.Int(),
                        CreatedDateTime = c.DateTime(nullable: false, precision: 0),
                        IsActive = c.Boolean(nullable: false),
                        Token = c.String(unicode: false),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        encryptedstring = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CreditDeductionLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        subscriptionId = c.Int(nullable: false),
                        CreditDeductionTypeId = c.Int(nullable: false),
                        JobId = c.Int(nullable: false),
                        Credits = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CreditDeductionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Credit = c.Double(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Plans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlanCode = c.String(unicode: false),
                        Name = c.String(unicode: false),
                        PlanTypeId = c.Int(nullable: false),
                        Description = c.String(unicode: false),
                        PlanExpiryDate = c.DateTime(nullable: false, precision: 0),
                        IsAutoRenewel = c.Boolean(),
                        CreditPrice = c.Double(),
                        Credit = c.Int(nullable: false),
                        NoOfAgents = c.Int(),
                        NoOfDrivers = c.Int(),
                        NoOfVehicles = c.Int(),
                        PerCreditSMSPrice = c.Double(),
                        CreatedDateTime = c.DateTime(nullable: false, precision: 0),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlanTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlanName = c.String(unicode: false),
                        SubscriptionTypeId = c.Int(nullable: false),
                        SubcriptionStatusCode = c.Int(nullable: false),
                        SubscriptionPrice = c.Double(nullable: false),
                        NoOfSmsCreditPurchase = c.Int(nullable: false),
                        SMSPrice = c.Double(nullable: false),
                        TotalPrice = c.Double(nullable: false),
                        TotalCredit = c.Int(nullable: false),
                        RemainingCredit = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false, precision: 0),
                        EndDate = c.DateTime(nullable: false, precision: 0),
                        NoOfAgents = c.Int(),
                        NoOfDrivers = c.Int(),
                        NoOfVehicles = c.Int(),
                        RemainingNoOfAgents = c.Int(),
                        RemainingNoOfDrivers = c.Int(),
                        RemainingNoOfVehicles = c.Int(),
                        PerCreditSMSPrice = c.Double(),
                        ChequeNo = c.String(unicode: false),
                        btTransactionId = c.String(unicode: false),
                        CreatedDateTime = c.DateTime(nullable: false, precision: 0),
                        IsActive = c.Boolean(nullable: false),
                        AccountId = c.Int(nullable: false),
                        PlanId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubscriptionStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubscriptionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "ClientId", "dbo.Clients");
            DropIndex("dbo.Accounts", new[] { "ClientId" });
            DropTable("dbo.SubscriptionTypes");
            DropTable("dbo.SubscriptionStatus");
            DropTable("dbo.Subscriptions");
            DropTable("dbo.PlanTypes");
            DropTable("dbo.Plans");
            DropTable("dbo.CreditDeductionTypes");
            DropTable("dbo.CreditDeductionLogs");
            DropTable("dbo.Clients");
            DropTable("dbo.Accounts");
        }
    }
}
