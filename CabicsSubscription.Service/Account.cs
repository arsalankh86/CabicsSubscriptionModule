using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Nullable<int> BalanceCredit { get; set; }
        public Nullable<int> AllCredit { get; set; }
        public Nullable<int> CurrentSubscriptionId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsActive { get; set; }

    }
}
