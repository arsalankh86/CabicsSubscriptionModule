using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class Plan
    {
        [Key]
        public int Id { get; set; }
        public string PlanCode { get; set; }
        public string Name { get; set; }
        public int PlanTypeId { get; set; }
        public string Description { get; set; }
        public DateTime PlanExpiryDate { get; set; }
        public Nullable<bool> IsAutoRenewel { get; set; }
        public Nullable<double> CreditPrice { get; set; }
        public int Credit { get; set; }
        public Nullable<int> NoOfAgents { get; set; }
        public Nullable<int> NoOfDrivers { get; set; }
        public Nullable<int> NoOfVehicles { get; set; }
        public Nullable<double> PerCreditSMSPrice { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public bool IsActive { get; set; }
       

        
    }
}
