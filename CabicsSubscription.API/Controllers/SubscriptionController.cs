using CabicsSubscription.API.Models;
using CabicsSubscription.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CabicsSubscription.Service.Services;
using Braintree;
using System.Configuration;

namespace CabicsSubscription.API.Controllers
{
    public class SubscriptionController : ApiController
    {

        [HttpPost]
        public Service.Subscription InsertSubscription(InsertSubscriptionRequest insertSubscriptionRequest)
        {
            return null;
         
        }

        [HttpPost]
        public Service.Subscription InsertSubscriptionbyAdmin(InsertSubscriptionByAdminRequest insertSubscriptionByAdminRequest)
        {
            SubscriptionService subscriptionService = new SubscriptionService();
            Service.Subscription subscription = null; //subscriptionService.PurchaseSubscription(insertSubscriptionByAdminRequest.PlanId, insertSubscriptionByAdminRequest.totalamount, insertSubscriptionByAdminRequest.cabofficeid, insertSubscriptionByAdminRequest.qty,"");
            return subscription;            
        }


        public List<CreditDeductionType> GetAllCreditDeductionDetail()
        {
            SubscriptionService subscriptionService = new SubscriptionService();
            return subscriptionService.GetCreditDeductionDetail();

            
        }

        [HttpGet]
        public List<Service.Subscription> GetUserAllSubscriptionDetail(int cabOfficeId)
        {
            SubscriptionService subscriptionService = new SubscriptionService();
            return subscriptionService.GetUserAllSubscriptionDetail(cabOfficeId);
        }

        [HttpPost]
        public bool RefundSubscription(RefundRequest refundRequest)
        {

            Braintree.Environment environment;
            if (ConfigurationManager.AppSettings["BtEnvironmentTestMode"].ToString() == "1")
                environment = Braintree.Environment.SANDBOX;
            else
                environment = Braintree.Environment.PRODUCTION;


            var gateway = new BraintreeGateway
            {
                Environment = environment,
                MerchantId = ConfigurationManager.AppSettings["BtMerchantId"],
                PublicKey = ConfigurationManager.AppSettings["BtPublicKey"],
                PrivateKey = ConfigurationManager.AppSettings["BtPrivateKey"]
            };

           var refundResult = gateway.Transaction.Refund(refundRequest.TransactionId, Convert.ToDecimal(refundRequest.Amount));
           return refundResult.IsSuccess();

        }

        [HttpGet]
        public List<Service.Subscription> ShowCurrentSubscription(string userguid)
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
