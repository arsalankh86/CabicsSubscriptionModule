using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CabicsSubscription.API.Models
{
    public class InsertSubscriptionByAdminRequest
    {

        public int PlanId { get; set; }
        public int qty { get; set; }
        public double totalamount { get; set; }
        public string chequeNo { get; set; }
        public int cabofficeid { get; set; }

    }
}