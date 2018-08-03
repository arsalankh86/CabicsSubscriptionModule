using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service.Model
{
    public class CustomCreditDeductionLog
    {
        public string FullName { get; set; }
        public string PlanName { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Credits { get; set; }
        public int JobId { get; set; }

    }
}
