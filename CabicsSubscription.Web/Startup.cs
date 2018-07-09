using Hangfire;
using Microsoft.Owin;
using Owin;
using System;


[assembly: OwinStartupAttribute(typeof(CabicsSubscription.Web.Startup))]

namespace CabicsSubscription.Web

{

    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {

            GlobalConfiguration.Configuration.UseSqlServerStorage("CabicsSubscriptionSQLDB");
            BackgroundJob.Enqueue(() => Console.WriteLine("Getting Started with HangFire!"));
            app.UseHangfireDashboard();
            app.UseHangfireServer();

        }

        private void ConfigureAuth(IAppBuilder app)
        {
        }
    }

}