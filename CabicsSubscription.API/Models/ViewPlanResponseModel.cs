using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CabicsSubscription.API.Models
{
    public class ViewPlanResponseModel
    {
        public int Id { get; set; }
        public int PlanCode { get; set; }
        public string Name { get; set; }
        public int PlanTypeId { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsAutoRenewel { get; set; }
        public Nullable<double> CreditPrice { get; set; }
        public int Credit { get; set; }
        public Nullable<int> NoOfAgents { get; set; }
        public Nullable<int> NoOfDrivers { get; set; }
        public Nullable<int> NoOfVehicles { get; set; }
        public Nullable<double> PerSMSPrice { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsActive { get; set; }
    }
}