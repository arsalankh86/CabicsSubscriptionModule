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
            AutomaticMigrationsEnabled = false;
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
        }
    }
}
