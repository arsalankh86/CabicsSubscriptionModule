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


        public Subscription PurchaseSubscription(int planId, double totalAmonut, int cabOfficeId, int qty, string chequeNo)
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
            if (plan.PlanTypeId == (int)Constant.PlayType.PayAsYouGo)
            {
                subscription.TotalCredit = qty;
                subscription.RemainingCredit = qty;
            }
            subscription.SubcriptionStatusCode = (int)Constant.SubscriptionStatus.Active;

            int subscriptionId = InsertSubscription(subscription);

            AccountService accountService = new AccountService();
            accountService.UpdateActiveSubsctionForAccount(subscriptionId, cabOfficeId);

            return subscription;


        }


        public Subscription PurchaseSubscription(int planId, double totalAmonut, int cabOfficeId, int qty, string chequeNo, int smscreditqty, double smscreditamount)
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
                subscription.CreatedDateTime = DateTime.Now;
                subscription.IsActive = true;
                subscription.NoOfAgents = plan.NoOfAgents;
                subscription.NoOfDrivers = plan.NoOfDrivers;
                subscription.NoOfVehicles = plan.NoOfVehicles;
                subscription.PerSMSPrice = plan.SMSCreditPrice;
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

            int subscriptionId = InsertSubscription(subscription);

            AccountService accountService = new AccountService();
            accountService.UpdateActiveSubsctionForAccount(subscriptionId, cabOfficeId);

            return subscription;


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
