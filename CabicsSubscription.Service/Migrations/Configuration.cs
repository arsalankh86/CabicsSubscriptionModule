namespace CabicsSubscription.Service.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CabicsSubscription.Service.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;            
        }

        protected override void Seed(CabicsSubscription.Service.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


            /// Defualt Client Insertion
            Client client = new Client();
            client.Id = 1;
            client.Name = "Cabics";
            client.Description = "Cabics";
            client.encryptedstring = Guid.NewGuid().ToString();
            context.Clients.AddOrUpdate(client);
            context.SaveChanges();

            /// Defualt Client Insertion
            CreditDeductionType dailyCreditDeductionType = new CreditDeductionType();
            dailyCreditDeductionType.Id = 1;
            dailyCreditDeductionType.Name = "Daily charges";
            dailyCreditDeductionType.Credit = 50;
            dailyCreditDeductionType.IsActive = true;
            dailyCreditDeductionType.CreatedDate = DateTime.Now;
            context.CreditDeductionTypes.AddOrUpdate(dailyCreditDeductionType);
            context.SaveChanges();

            /// Per Job Client Insertion
            CreditDeductionType jobCreditDeductionType = new CreditDeductionType();
            jobCreditDeductionType.Id = 2;
            jobCreditDeductionType.Name = "Per Job charges";
            jobCreditDeductionType.Credit = 3;
            jobCreditDeductionType.IsActive = true;
            jobCreditDeductionType.CreatedDate = DateTime.Now;
            context.CreditDeductionTypes.AddOrUpdate(jobCreditDeductionType);
            context.SaveChanges();

            /// Per SMS Client Insertion
            CreditDeductionType smsCreditDeductionType = new CreditDeductionType();
            smsCreditDeductionType.Id = 3;
            smsCreditDeductionType.Name = "Daily charges";
            smsCreditDeductionType.Credit = 6;
            smsCreditDeductionType.IsActive = true;
            smsCreditDeductionType.CreatedDate = DateTime.Now;
            context.CreditDeductionTypes.AddOrUpdate(smsCreditDeductionType);
            context.SaveChanges();



        }
    }
}
