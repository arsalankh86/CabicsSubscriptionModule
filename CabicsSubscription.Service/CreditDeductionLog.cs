using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class CreditDeductionLog
    {
        public int AccountId { get; set; }
        public int subscriptionId { get; set; }
        public int CreditDeductionTypeId { get; set; }
        public int JobId { get; set; }
        public int Credits { get; set; }

    }
}
