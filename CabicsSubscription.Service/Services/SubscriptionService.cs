using Hangfire;
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
                context.Subscriptions.Add(subscription);
                context.SaveChanges();
                return subscription.Id;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public Subscription GetSubscriptionBySubscriptionId(int subscriptionId)
        {
            DataContext context = new DataContext();
            return context.Subscriptions.FirstOrDefault(x => x.Id == subscriptionId && x.IsActive == true);
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

     

        //public int PurchaseSubscription(int planId, double totalAmonut, int cabOfficeId, int qty, string chequeNo)
        //{
        //    PlanService planService = new PlanService();
        //    Plan plan = planService.GetPlanDetail(planId);

        //    Subscription subscription = new Subscription();
        //    subscription.PlanId = plan.Id;
        //    subscription.PlanName = plan.Name;
        //    subscription.StartDate = DateTime.Now;
        //    subscription.TotalPrice = totalAmonut;
        //    subscription.AccountId = cabOfficeId;
        //    subscription.SubscriptionTypeId = plan.PlanTypeId;
        //    if (plan.PlanTypeId == (int)Constant.PlayType.Monthly)
        //    {
        //        subscription.EndDate = DateTime.Now.AddMonths(1);
        //        subscription.IsActive = true;
        //        subscription.NoOfAgents = plan.NoOfAgents;
        //        subscription.NoOfDrivers = plan.NoOfDrivers;
        //        subscription.NoOfVehicles = plan.NoOfVehicles;
        //        subscription.PerCreditSMSPrice = plan.PerCreditSMSPrice;
        //        subscription.RemainingNoOfAgents = plan.NoOfAgents;
        //        subscription.RemainingNoOfDrivers = plan.NoOfDrivers;
        //        subscription.RemainingNoOfVehicles = plan.NoOfVehicles;
        //    }
        //    if (plan.PlanTypeId == (int)Constant.PlayType.PayAsYouGo)
        //    {
        //        subscription.TotalCredit = qty;
        //        subscription.RemainingCredit = qty;
        //    }
        //    subscription.SubcriptionStatusCode = (int)Constant.SubscriptionStatus.Active;
        //    subscription.CreatedDateTime = DateTime.Now;

        //    int subscriptionId = InsertSubscription(subscription);

        //    AccountService accountService = new AccountService();
        //    accountService.UpdateActiveSubsctionForAccount(subscriptionId, cabOfficeId);

        //    return subscriptionId;


        //}

        public Subscription GetSubscriptionByTransactionId(string transactionId)
        {
            DataContext context = new DataContext();
            Subscription subscription = context.Subscriptions.FirstOrDefault(x => x.IsActive == true && x.btTransactionId == transactionId);
            return subscription;
        }

        public void DeactivateCurrentSubscription(string transactionId)
        {
            DataContext context = new DataContext();
            Subscription subscription = context.Subscriptions.FirstOrDefault(x => x.IsActive == true && x.btTransactionId == transactionId);
            subscription.IsActive = false;
            context.SaveChanges();
        }

        public List<Subscription> GetCredititUtilizationReport(int cabOfficeId, int subscriptionId)
        {
            throw new NotImplementedException();
        }

        public void MinusCreditFromSubscription(string transactionId, double amount)
        {
            DataContext context = new DataContext();
            Subscription subscription = context.Subscriptions.FirstOrDefault(x => x.IsActive == true && x.btTransactionId == transactionId);
            double subscriptionPrice = subscription.SubscriptionPrice;
            double finalPrice = subscriptionPrice - amount;

            //double totalCredit = finalPrice * subscription.TotalCredit;
            //double remianingCredit = totalCredit - subscription.RemainingCredit;


            //subscription.RemainingCredit = remianingCredit;
            //subscription.TotalCredit = totalCredit;
            //context.SaveChanges();

        }

        public List<CreditDeductionType> GetCreditDeductionDetail()
        {
            using (DataContext context = new DataContext())
            {
                List<CreditDeductionType> lstCreditDeductionType = context.CreditDeductionTypes.Where(x => x.IsActive == true).ToList();
                return lstCreditDeductionType;
            }
        }

        public List<Subscription> GetUserAllSubscriptionDetail(int cabOfficeId)
        {
            DataContext context = new DataContext();
            return context.Subscriptions.Where(x => x.IsActive == true && x.AccountId == cabOfficeId).ToList();
        }

        public void UpdateTotalCreditAndAmount(int id, double totalPrice, double totalCredit)
        {
            using (DataContext context = new DataContext())
            {

                Subscription subscription = context.Subscriptions.FirstOrDefault(x => x.Id == id && x.IsActive == true);
                subscription.TotalPrice = totalPrice;
                subscription.TotalCredit = Convert.ToInt32(totalCredit);
                context.SaveChanges();

            }
        }

        public List<Subscription> ShowCurrentSubscription(string userguid)
        {
            DataContext context = new DataContext();
            var account = context.Accounts.FirstOrDefault(x => x.Token == userguid && x.IsActive == true);

            List<Subscription> subscription = new List<Subscription>();
            if (account != null)
                subscription = context.Subscriptions.Where(x => x.AccountId == account.Id).OrderByDescending(x=>x.IsActive).ToList();

            return subscription;

        }

        public int PurchaseSubscription(int planId, double totalAmonut, int cabOfficeId, int qty, string chequeNo, 
            int smscreditqty, double smscreditamount, string transactionId, string btSubsccriptionId, bool isAutoRenewel, int noOfBillingCycle)
        {

            /// If plan is pay as you go and this user alreasy have subscription
            /// 

            DataContext context = new DataContext();
            PlanService planService = new PlanService();
            Plan plan = planService.GetPlanDetail(planId);

            Subscription subscription = null;
            if (plan.PlanTypeId == (int)Constant.SubscriptionType.PayAsYouGo)
                subscription = context.Subscriptions.FirstOrDefault(x => x.IsActive == true && x.AccountId == cabOfficeId && x.SubscriptionTypeId == (int)Constant.SubscriptionType.PayAsYouGo);

            int subscriptionId = 0;

            if (subscription == null)
            {
                subscription = new Subscription();
               

                subscription.PlanId = plan.Id;
                subscription.PlanName = plan.Name;
                subscription.StartDate = DateTime.UtcNow;
                subscription.TotalPrice = totalAmonut;
                subscription.AccountId = cabOfficeId;
                subscription.SubscriptionTypeId = plan.PlanTypeId;
                subscription.btTransactionId = transactionId;
                if (plan.PlanTypeId == (int)Constant.PlayType.Monthly)
                {
                    subscription.EndDate = DateTime.UtcNow.AddMonths(1);
                    subscription.NoOfAgents = plan.NoOfAgents;
                    subscription.NoOfDrivers = plan.NoOfDrivers;
                    subscription.NoOfVehicles = plan.NoOfVehicles;
                    subscription.PerCreditSMSPrice = plan.PerCreditSMSPrice;
                    subscription.RemainingNoOfAgents = plan.NoOfAgents;
                    subscription.RemainingNoOfDrivers = plan.NoOfDrivers;
                    subscription.RemainingNoOfVehicles = plan.NoOfVehicles;
                    subscription.SMSPrice = smscreditamount;
                    subscription.NoOfSmsCreditPurchase = smscreditqty;
                    subscription.IsAutoRenewel = isAutoRenewel;
                    subscription.NoOfBillingCycle = noOfBillingCycle;
                    subscription.btSubscriptionId = btSubsccriptionId;
                }
                if (plan.PlanTypeId == (int)Constant.PlayType.PayAsYouGo)
                {
                    
                    subscription.TotalCredit = qty;
                    subscription.RemainingCredit = qty;
                 
                }
                
                subscription.SubcriptionStatusCode = (int)Constant.SubscriptionStatus.Active;
                subscription.IsActive = true;
                subscription.Status = Constant.SubscriptionStatus.Active.ToString();
                subscription.ChequeNo = chequeNo;
                subscription.CreatedDateTime = DateTime.UtcNow;

                if (plan.PlanTypeId == 1)
                   subscription.IsAutoRenewel = false;

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

            AutomatedService automatedService = new AutomatedService();
            if(plan.PlanTypeId == (int)Constant.PlayType.PayAsYouGo)
                RecurringJob.AddOrUpdate(() => automatedService.DeductDailyCredit(subscriptionId), Cron.Daily);

            return subscriptionId;


        }

        public void UpdateSubscriptionCredit(int subscriptionId, int dailyCreditDeductionCredit, int deductiontypes)
        {
            using (DataContext context = new DataContext())
            {
                Subscription upSubscription = context.Subscriptions.FirstOrDefault(x => x.Id == subscriptionId && x.IsActive == true);
                upSubscription.RemainingCredit = upSubscription.RemainingCredit - dailyCreditDeductionCredit;
                context.SaveChanges();

                CreditDeductionLog creditDeductionLog = new CreditDeductionLog();
                creditDeductionLog.AccountId = upSubscription.Id;
                creditDeductionLog.subscriptionId = upSubscription.Id;
                creditDeductionLog.CreditDeductionTypeId = deductiontypes;
                creditDeductionLog.CreatedDate = DateTime.UtcNow;
                context.CreditDeductionLogs.Add(creditDeductionLog);
                context.SaveChanges();
            }
        }

        public void UpdateSubscriptionRemainingMonthlySMSCredit(int subscriptionId, int dailyCreditDeductionCredit, int deductiontypes)
        {
            using (DataContext context = new DataContext())
            {
                Subscription upSubscription = context.Subscriptions.FirstOrDefault(x => x.Id == subscriptionId && x.IsActive == true);
                upSubscription.RemainingSmsCreditPurchase = upSubscription.RemainingSmsCreditPurchase - dailyCreditDeductionCredit;
                context.SaveChanges();

                CreditDeductionLog creditDeductionLog = new CreditDeductionLog();
                creditDeductionLog.AccountId = upSubscription.Id;
                creditDeductionLog.subscriptionId = upSubscription.Id;
                creditDeductionLog.CreditDeductionTypeId = deductiontypes;
                creditDeductionLog.CreatedDate = DateTime.UtcNow;
                context.CreditDeductionLogs.Add(creditDeductionLog);
                context.SaveChanges();
            }
        }

        public CreditDeductionType GetCreditDailyDeductionDetail()
        {
            using (DataContext context = new DataContext())
            {
                CreditDeductionType creditDeductiontype = context.CreditDeductionTypes.FirstOrDefault(x => x.IsActive == true && x.Id == (int)Constant.CreditDeductionType.DailyCharges);
                return creditDeductiontype;

            }
        }

        public CreditDeductionType GetCreditJobDeductionDetail()
        {
            using (DataContext context = new DataContext())
            {
                CreditDeductionType creditDeductiontype = context.CreditDeductionTypes.FirstOrDefault(x => x.IsActive == true && x.Id == (int)Constant.CreditDeductionType.PerJobCharges);
                return creditDeductiontype;

            }
        }

        public CreditDeductionType GetCreditSMSDeductionDetail()
        {
            using (DataContext context = new DataContext())
            {
                CreditDeductionType creditDeductiontype = context.CreditDeductionTypes.FirstOrDefault(x => x.IsActive == true && x.Id == (int)Constant.CreditDeductionType.SMSCharges);
                return creditDeductiontype;

            }
        }

    }
}
