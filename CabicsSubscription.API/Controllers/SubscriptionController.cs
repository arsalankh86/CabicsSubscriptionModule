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

        [HttpPost]
        public Subscription InsertSubscriptionbyAdmin(InsertSubscriptionByAdminRequest insertSubscriptionByAdminRequest)
        {
            SubscriptionService subscriptionService = new SubscriptionService();
            PlanService planService = new PlanService();
            Plan plan = planService.GetPlanDetail(insertSubscriptionByAdminRequest.PlanId);

            Subscription subscription = new Subscription();
            subscription.PlanId = plan.Id;
            subscription.PlanName = plan.Name;
            subscription.StartDate = DateTime.Now;
            subscription.TotalPrice = insertSubscriptionByAdminRequest.totalamount;
            subscription.AccountId = insertSubscriptionByAdminRequest.cabofficeid;
            subscription.SubscriptionTypeId = plan.PlanTypeId;
            if(plan.PlanTypeId == (int)Constant.PlayType.Monthly)
            {
                subscription.EndDate = DateTime.Now.AddMonths(1);
                subscription.CreatedDateTime = DateTime.Now;
                subscription.IsActive = true;
                subscription.NoOfAgents = plan.NoOfAgents;
                subscription.NoOfDrivers = plan.NoOfDrivers;
                subscription.NoOfVehicles = plan.NoOfVehicles;
                subscription.PerSMSPrice = plan.PerSMSPrice;
                subscription.RemainingNoOfAgents = plan.NoOfAgents;
                subscription.RemainingNoOfDrivers = plan.NoOfDrivers;
                subscription.RemainingNoOfVehicles = plan.NoOfVehicles;
            }
            if(plan.PlanTypeId == (int)Constant.PlayType.PayAsYouGo)
            {
                subscription.TotalCredit = insertSubscriptionByAdminRequest.qty;
                subscription.RemainingCredit = insertSubscriptionByAdminRequest.qty;
            }
            subscription.SubcriptionStatusCode = (int)Constant.SubscriptionStatus.Pending;

           int subscriptionId= subscriptionService.InsertSubscription(subscription);

            AccountService accountService = new AccountService();
            accountService.UpdateActiveSubsctionForAccount(subscriptionId, insertSubscriptionByAdminRequest.cabofficeid);

            

            return subscription;
            
        }
    }
}
