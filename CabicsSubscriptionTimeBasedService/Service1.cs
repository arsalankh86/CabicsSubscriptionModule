using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CabicsSubscription.Service;

namespace CabicsSubscriptionTimeBasedService
{
    public partial class Service1 : ServiceBase
    {

        private Timer timer = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Diagnostics.Debugger.Launch();

            WindowsServiceLogging.WriteEventLog("Timer is set for one day");
            WindowsServiceLogging.WriteEventLog("Service Started");
            WindowsServiceLogging.WriteEventLog("*******************************************************************************************");
            WindowsServiceLogging.WriteEventLog("*******************************************************************************************");
            WindowsServiceLogging.WriteEventLog("*******************************************************************************************");
            timer = new Timer();
            this.timer.Interval = 10000;
            this.timer.Elapsed += new ElapsedEventHandler(this.timer_tick);
            this.timer.Enabled = true;
        }

        private void timer_tick(object sender, ElapsedEventArgs e)
        {
            WindowsServiceLogging.WriteEventLog("******************* -- Event: OnStart -- ***********************");
            WindowsServiceLogging.WriteEventLog("Timer tick and operation started");


            AutomatedService automatedService = new AutomatedService();
            automatedService.ExecutonFunction();


            WindowsServiceLogging.WriteEventLog("Operation Completed Succesfully");
            WindowsServiceLogging.WriteEventLog("******************* -- Event: Completed -- ***********************");
        }



        protected override void OnStop()
        {
            WindowsServiceLogging.WriteEventLog("******************* -- Event: OnStop -- ***********************");
            WindowsServiceLogging.WriteEventLog("Attempt to Shut Down the Service");
            timer.Stop();
            timer = null;
            WindowsServiceLogging.WriteEventLog("Service Shutdown by User");
        }
    }
}
