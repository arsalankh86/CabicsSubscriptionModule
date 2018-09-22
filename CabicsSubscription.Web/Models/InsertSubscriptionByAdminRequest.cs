using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CabicsSubscription.Web.Models
{

    public class InsertSubscriptionByAdminRequest
    {

        public int PlanId { get; set; }
        public int qty { get; set; }
        public int smscreditqty { get; set; }
        public double hdnsmscreditamount { get; set; }
        public double totalamount { get; set; }
        public string chequeNo { get; set; }
        public int cabofficeid { get; set; }
        public string hdnamount { get; set; }
        public string hdnsmscreditotaltamount { get; set; }
        public string chkautorenewel { get; set; }

    }
}