using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CabicsSubscription.API.Models
{
    public class RefundRequest
    {
        public string TransactionId { get; set; }
        public double Amount { get; set; }
    }
}