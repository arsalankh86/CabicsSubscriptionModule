using CabicsSubscription.Service.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service.Services
{
    public class MigrationService
    {
        public static void RunMigration()
        {
            
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
            DataContext context = new DataContext();
            context.Database.Initialize(true);

            var configuration = new CabicsSubscription.Service.Migrations.Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }
    }
}
