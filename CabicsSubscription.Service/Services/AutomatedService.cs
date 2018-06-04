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
                // Purchase using Braintree
                var gateway = new BraintreeGateway
                {
                    Environment = Braintree.Environment.SANDBOX,
                    MerchantId = "gbhg9d4dvt83v4ff",
                    PublicKey = "n4yhd55nx6g2ygdn",
                    PrivateKey = "374d896ef9682b3550a76d2b82811d1a"
                };
                
                var request = new TransactionRequest
                {
                    Amount = Convert.ToDecimal(amount),
                    PaymentMethodNonce = nonce,
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    }
                };

                Result<Transaction> result = gateway.Transaction.Sale(request);
                if (result.IsSuccess())
                {
                    Transaction transaction = result.Target;
                
                  
                    subscriptionService.PurchaseSubscription(planId, amount, cabOfficeId, qty, "", smscreditqty, hdnsmscreditamount, transaction.Id);
                    subscriptionService.DeactivateCurrentSubscription(transactionId);
                }
                else if (result.Transaction != null)
                {
                    //return RedirectToAction("Show", new { id = result.Transaction.Id });
                }
                else
                {
                }
            }



        }

    }
}
