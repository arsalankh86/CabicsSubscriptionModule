using Braintree;
using CabicsSubscription.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class AutomatedService
    {
        public void MarkAutoRenewalSubscription(int subscriptionId)
        {


            SubscriptionService subscriptionService = new SubscriptionService();
            AccountService accountService = new AccountService();

            Subscription subscription = subscriptionService.GetSubscriptionBySubscriptionId(subscriptionId);
            if (subscription.SubscriptionTypeId != (int)Constant.SubscriptionType.Monthly)
                return;

            int cabOfficeId = subscription.AccountId;
            Account account = accountService.getCabOfficeByCabOfficeId(cabOfficeId);
            int subscriptionTypeId = subscription.SubscriptionTypeId;
            int planId = subscription.PlanId;
            double amount = Convert.ToDouble(subscription.TotalPrice);
            var nonce = account.PaymentMethodNonce;

            DateTime startDate = subscription.StartDate;
            DateTime endDate = subscription.EndDate;
            DateTime todayDate = DateTime.Now;

            var qty = 1;
            int smscreditqty = Convert.ToInt32(subscription.NoOfSmsCreditPurchase);
            double hdnsmscreditamount = Convert.ToInt32(subscription.SMSPrice);
            string transactionId = subscription.btTransactionId;

            if (endDate == todayDate)
            {
                
            }



        }

    }
}
