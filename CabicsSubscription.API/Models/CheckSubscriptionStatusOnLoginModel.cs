using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CabicsSubscription.API.Models
{
    public class CheckSubscriptionStatusOnLoginModel
    {
        public string CabOfficeEmail { get; set; }
        public int CabOfficeId { get; set; }
    }
}