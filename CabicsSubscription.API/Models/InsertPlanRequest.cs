using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CabicsSubscription.API.Models
{
    public class InsertPlanRequest
    {

        public int PlanCode { get; set; }
        public string Name { get; set; }
        public int PlanTypeId { get; set; }
        public string Description { get; set; }
        public Nullable<double> CreditPrice { get; set; }
        public int Credit { get; set; }
        public Nullable<int> NoOfAgents { get; set; }
        public Nullable<int> NoOfDrivers { get; set; }
        public Nullable<int> NoOfVehicles { get; set; }
        public double PerSMSPrice { get; set; }
    }
}