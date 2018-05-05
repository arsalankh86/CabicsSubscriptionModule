using CabicsSubscription.API.Models;
using CabicsSubscription.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CabicsSubscription.API.Controllers
{
    public class SubscriptionController : ApiController
    {

        [HttpPost]
        public Subscription InsertSubscription(InsertSubscriptionRequest insertSubscriptionRequest)
        {
            return null;
         
        }
    }
}
