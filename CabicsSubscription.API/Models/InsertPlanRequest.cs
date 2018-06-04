using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CabicsSubscription.API.Models
{
    public class InsertPlanRequest
    {

        public string PlanCode { get; set; }
        public string Name { get; set; }
        public int PlanTypeId { get; set; }
        public string Description { get; set; }
        public double CreditPrice { get; set; }
        public int Credit { get; set; }
        public int NoOfAgents { get; set; }
        public int NoOfDrivers { get; set; }
        public int NoOfVehicles { get; set; }
        public double PerSMSPrice { get; set; }
        public string PlanExpiryDate { get; set; }
        public string BrainTreePlan { get; set; }

    }
}