using CabicsSubscription.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using static System.Data.Entity.Migrations.Model.UpdateDatabaseOperation;

namespace CabicsSubscription.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //// For Migration
         
            //CabicsSubscription.Service.Migrations.newsubscription newsubscriptionobj = new Service.Migrations.newsubscription();
            //newsubscriptionobj.Up();

            var configuration = new CabicsSubscription.Service.Migrations.Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();




        }
    }
}
