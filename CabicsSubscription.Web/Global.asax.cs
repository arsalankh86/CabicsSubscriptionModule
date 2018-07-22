using CabicsSubscription.Service;
using Hangfire;
//using Hangfire.MySql;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CabicsSubscription.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private BackgroundJobServer _backgroundJobServer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutomatedService automatedService = new AutomatedService();
            string sqlCon = System.Configuration.ConfigurationManager.AppSettings["SQLConnectionStrings"];
            //string mySqlCon = System.Configuration.ConfigurationManager.AppSettings["HangFireMYSQLConnectionStrings"];

            Hangfire.GlobalConfiguration.Configuration
                .UseSqlServerStorage(sqlCon);

            //Hangfire.GlobalConfiguration.Configuration.UseStorage(new MySqlStorage(mySqlCon));

            _backgroundJobServer = new BackgroundJobServer();

        }


    }
}
