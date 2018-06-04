using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service.Services
{
    public class SubscriptionService
    {
        public int InsertSubscription(Subscription subscription)
        {
            try
            {
                DataContext context = new DataContext();
                context.Subscription.Add(subscription);
                context.SaveChanges();
                return subscription.Id;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        internal Subscription GetSubscriptionBySubscriptionId(int subscriptionId)
        {
            throw new NotImplementedException();
        }

        public int InsertRefundTranactionLog(RefundTranactionLog refundTranactionLog)
        {
            try
            {
                DataContext context = new DataContext();
                context.RefundTranactionLogs.Add(refundTranactionLog);
                context.SaveChanges();
                return refundTranactionLog.Id;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public int PurchaseSubscription(int planId, double totalAmonut, int cabOfficeId, int qty, string chequeNo)
        {
            PlanService planService = new PlanService();
            Plan plan = planService.GetPlanDetail(planId);

            Subscription subscription = new Subscription();
            subscription.PlanId = plan.Id;
            subscription.PlanName = plan.Name;
            subscription.StartDate = DateTime.Now;
            subscription.TotalPrice = totalAmonut;
            subscription.AccountId = cabOfficeId;
            subscription.SubscriptionTypeId = plan.PlanTypeId;
            if (plan.PlanTypeId == (int)Constant.PlayType.Monthly)
            {
                subscription.EndDate = DateTime.Now.AddMonths(1);
                subscription.IsActive = true;
                subscription.NoOfAgents = plan.NoOfAgents;
                subscription.NoOfDrivers = plan.NoOfDrivers;
                subscription.NoOfVehicles = plan.NoOfVehicles;
                subscription.PerCreditSMSPrice = plan.PerCreditSMSPrice;
                subscription.RemainingNoOfAgents = plan.NoOfAgents;
                subscription.RemainingNoOfDrivers = plan.NoOfDrivers;
                subscription.RemainingNoOfVehicles = plan.NoOfVehicles;
            }
            if (plan.PlanTypeId == (int)Constant.PlayType.PayAsYouGo)
            {
                subscription.TotalCredit = qty;
                subscription.RemainingCredit = qty;
            }
            subscription.SubcriptionStatusCode = (int)Constant.SubscriptionStatus.Active;
            subscription.CreatedDateTime = DateTime.Now;

            int subscriptionId = InsertSubscription(subscription);

            AccountService accountService = new AccountService();
            accountService.UpdateActiveSubsctionForAccount(subscriptionId, cabOfficeId);

            return subscriptionId;


        }

       

        public void DeactivateCurrentSubscription(string transactionId)
        {
            DataContext context = new DataContext();
            Subscription subscription = context.Subscription.FirstOrDefault(x => x.IsActive == true && x.btTransactionId == transactionId);
            subscription.IsActive = false;
            context.SaveChanges();
        }


        public void MinusCreditFromSubscription(string transactionId, double amount)
        {
            DataContext context = new DataContext();
            Subscription subscription = context.Subscription.FirstOrDefault(x => x.IsActive == true && x.btTransactionId == transactionId);
            double subscriptionPrice = subscription.SubscriptionPrice;
            double finalPrice = subscriptionPrice - amount;

            //double totalCredit = finalPrice * subscription.TotalCredit;
            //double remianingCredit = totalCredit - subscription.RemainingCredit;


            //subscription.RemainingCredit = remianingCredit;
            //subscription.TotalCredit = totalCredit;
            //context.SaveChanges();

        }


        public List<Subscription> GetUserAllSubscriptionDetail(int cabOfficeId)
        {
            DataContext context = new DataContext();
            return context.Subscription.Where(x => x.IsActive == true && x.AccountId == cabOfficeId).ToList();
        }

        public List<Subscription> ShowCurrentSubscription(string userguid)
        {
            DataContext context = new DataContext();
            var account = context.Accounts.FirstOrDefault(x => x.Token == userguid && x.IsActive == true);

            List<Subscription> subscription = new List<Subscription>();
            if (account != null)
                subscription = context.Subscription.Where(x => x.AccountId == account.Id && x.IsActive == true).ToList();

            return subscription;

        }

        public int PurchaseSubscription(int planId, double totalAmonut, int cabOfficeId, int qty, string chequeNo, 
            int smscreditqty, double smscreditamount, string transactionId, string btSubsccriptionId, bool isAutoRenewel, int noOfBillingCycle)
        {

            /// If plan is pay as you go and this user alreasy have subscription
            /// 

            DataContext context = new DataContext();
            Subscription subscription = context.Subscription.FirstOrDefault(x => x.IsActive == true && x.AccountId == cabOfficeId && x.SubscriptionTypeId == (int)Constant.SubscriptionType.PayAsYouGo);
            int subscriptionId = 0;

            if (subscription == null)
            {
                PlanService planService = new PlanService();
                Plan plan = planService.GetPlanDetail(planId);

                subscription.PlanId = plan.Id;
                subscription.PlanName = plan.Name;
                subscription.StartDate = DateTime.Now;
                subscription.TotalPrice = totalAmonut;
                subscription.AccountId = cabOfficeId;
                subscription.SubscriptionTypeId = plan.PlanTypeId;
                subscription.btTransactionId = transactionId;
                if (plan.PlanTypeId == (int)Constant.PlayType.Monthly)
                {
                    subscription.EndDate = DateTime.Now.AddMonths(1);
                    subscription.CreatedDateTime = DateTime.Now;
                    subscription.IsActive = true;
                    subscription.NoOfAgents = plan.NoOfAgents;
                    subscription.NoOfDrivers = plan.NoOfDrivers;
                    subscription.NoOfVehicles = plan.NoOfVehicles;
                    subscription.PerCreditSMSPrice = plan.PerCreditSMSPrice;
                    subscription.RemainingNoOfAgents = plan.NoOfAgents;
                    subscription.RemainingNoOfDrivers = plan.NoOfDrivers;
                    subscription.RemainingNoOfVehicles = plan.NoOfVehicles;
                    subscription.SMSPrice = smscreditamount;
                    subscription.NoOfSmsCreditPurchase = smscreditqty;
                }
                if (plan.PlanTypeId == (int)Constant.PlayType.PayAsYouGo)
                {
                 
                }
                subscription.IsAutoRenewel = isAutoRenewel;
                subscription.NoOfBillingCycle = noOfBillingCycle;
                subscription.btSubscriptionId = btSubsccriptionId;
                subscription.SubcriptionStatusCode = (int)Constant.SubscriptionStatus.Active;
                subscription.IsActive = true;

                subscriptionId = InsertSubscription(subscription);

                AccountService accountService = new AccountService();
                accountService.UpdateActiveSubsctionForAccount(subscriptionId, cabOfficeId);
            }
            else
            {
                subscriptionId = subscription.Id;
                subscription.TotalCredit = subscription.TotalCredit + qty;
                subscription.RemainingCredit = subscription.RemainingCredit + qty;
                subscription.TotalPrice = subscription.TotalPrice + totalAmonut;
                context.SaveChanges();
            }


            return subscriptionId;


        }

        public List<CreditDeductionType> GetCreditDeductionDetail()
        {
            using (DataContext context = new DataContext())
            {
                List<CreditDeductionType> creditDeductiontype = context.CreditDeductionTypes.Where(x => x.IsActive == true).ToList();
                return creditDeductiontype;

            }
        }

    }
}
