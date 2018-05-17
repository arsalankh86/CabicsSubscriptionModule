using CabicsSubscription.API.Models;
using CabicsSubscription.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CabicsSubscription.Service.Services;

namespace CabicsSubscription.API.Controllers
{
    public class SubscriptionController : ApiController
    {

        [HttpPost]
        public Subscription InsertSubscription(InsertSubscriptionRequest insertSubscriptionRequest)
        {
            return null;
         
        }

        [HttpPost]
        public Subscription InsertSubscriptionbyAdmin(InsertSubscriptionByAdminRequest insertSubscriptionByAdminRequest)
        {
            SubscriptionService subscriptionService = new SubscriptionService();
            Subscription subscription = subscriptionService.PurchaseSubscription(insertSubscriptionByAdminRequest.PlanId, insertSubscriptionByAdminRequest.totalamount, insertSubscriptionByAdminRequest.cabofficeid, insertSubscriptionByAdminRequest.qty,"");
            return subscription;            
        }

        public List<CreditDeductionType> GetAllCreditDeductionDetail()
        {
            SubscriptionService subscriptionService = new SubscriptionService();
            return subscriptionService.GetCreditDeductionDetail();

            
        }

        public List<Subscription> GetUserAllSubscriptionDetail(string userguid)
        {
            SubscriptionService subscriptionService = new SubscriptionService();

            return null;
        }

        [HttpGet]
        public Subscription ShowCurrentSubscription(string userguid)
        {
            if (userguid != "undefined")
            {
                SubscriptionService subscriptionService = new SubscriptionService();
                return subscriptionService.ShowCurrentSubscription(userguid);
            }
            return null;
        }


    }
}
