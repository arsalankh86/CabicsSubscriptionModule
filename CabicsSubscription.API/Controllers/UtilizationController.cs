using CabicsSubscription.API.Models;
using CabicsSubscription.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CabicsSubscription.Service.Services;
using System.Globalization;

namespace CabicsSubscription.API.Controllers
{
    public class UtilizationController : ApiController
    {
        PlanService planService = new PlanService();


        public UtilizeSubscriptionResponse UtilizeSubscription(UtilizeSubscriptionModel utilizeSubscriptionModel)
        {
            UtilizeSubscriptionResponse utilizeSubscriptionResponse = new UtilizeSubscriptionResponse();
            AccountService accountService = new AccountService();
            SubscriptionService subscriptionService = new SubscriptionService();
            Account account = null;
            var token = "";
            if (Request.Headers.Contains("authtoken")) {
                IEnumerable<string> headerValues = Request.Headers.GetValues("authtoken");
                token = headerValues.FirstOrDefault();
                account =  accountService.getCabOfficeByToken(token);
                if(account == null)
                {
                    utilizeSubscriptionResponse.Error = Constant.APIError.AccountNotFound.ToString();
                    utilizeSubscriptionResponse.ErrorCode = (int)Constant.APIError.AccountNotFound;
                    return utilizeSubscriptionResponse;
                }

                /// Check this account is able to create a job
                Subscription subscription = subscriptionService.GetSubscriptionBySubscriptionId(Convert.ToInt32(account.CurrentSubscriptionId));
                
                if(subscription == null)
                {
                    utilizeSubscriptionResponse.Error = Constant.APIError.NoSubscriptionFound.ToString();
                    utilizeSubscriptionResponse.ErrorCode = (int)Constant.APIError.NoSubscriptionFound;
                    return utilizeSubscriptionResponse;
                }

                if (subscription == null)
                {
                    utilizeSubscriptionResponse.Error = Constant.APIError.NoSubscriptionFound.ToString();
                    utilizeSubscriptionResponse.ErrorCode = (int)Constant.APIError.NoSubscriptionFound;
                    return utilizeSubscriptionResponse;
                }

                int subscriptionType = subscription.SubscriptionTypeId;
                if(subscriptionType == 1)
                {
                  // List<CreditDeductionLog> creditDeductionLog = subscriptionService.GetCreditDeductionDetail();
                }

               

                //CreditDeductionLog creditDeductionLog = new CreditDeductionLog();
                //creditDeductionLog.AccountId = account.Id;
                //creditDeductionLog.subscriptionId = 0;
                //creditDeductionLog.CreditDeductionTypeId = utilizeSubscriptionModel.CreditUtilizationType;
                //creditDeductionLog.CreatedDate = DateTime.UtcNow;
                
            }







            return utilizeSubscriptionResponse;

        }


        public List<Plan> GetAllPlan()
        {
           return planService.GetAllPlan();
        }


        public List<Plan> GetAllPlanForCabOffice()
        {
            return planService.GetAllPlanForCabOffice();
        }


        [HttpGet]
        public Plan GetPlanDetail(int planId)
        {
            return planService.GetPlanDetail(planId);
        }


        [HttpPost]
        public int InsertPlan(InsertPlanRequest planrequest)
        {
            int result = PlanAlreadyExist(planrequest.PlanCode, planrequest.PlanTypeId);

            if (result != 1)
                return result;

                Plan plan = new Plan();
            plan.Name = planrequest.Name;
            plan.PlanCode = planrequest.PlanCode;
            plan.Description = planrequest.Description;
            plan.Credit = planrequest.Credit;
            plan.CreditPrice = planrequest.CreditPrice;
            plan.PlanTypeId = planrequest.PlanTypeId;
            plan.NoOfAgents = planrequest.NoOfAgents;
            plan.NoOfDrivers = planrequest.NoOfDrivers;
            plan.NoOfVehicles = planrequest.NoOfVehicles;
            plan.PerCreditSMSPrice = planrequest.PerSMSPrice;
            plan.BrainTreePlanName = planrequest.BrainTreePlan;
            plan.IsActive = true;
            plan.CreatedDateTime = DateTime.Now;
            plan.UpdatedDateTime = DateTime.Now;
            plan.PlanExpiryDateString = planrequest.PlanExpiryDate.ToString();
            // plan.PlanExpiryDate = DateTime.Now.AddMonths(6); 
            //"7/10/2013"
            string date = planrequest.PlanExpiryDate.ToString();
            DateTime utcDate = DateTime.SpecifyKind(Convert.ToDateTime(date), DateTimeKind.Utc);
            plan.PlanExpiryDate = utcDate; //Convert.ToDateTime(date);
            //plan.PlanExpiryDate = DateTime.ParseExact(planrequest.PlanExpiryDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            plan = planService.InsertPlan(plan);
                return plan.Id;
        }

        [HttpPost]
        public int EditPlan(InsertPlanRequest planrequest)
        {
            Plan plan = new Plan();
            plan.Name = planrequest.Name;
            plan.PlanCode = planrequest.PlanCode;
            plan.Description = planrequest.Description;
            plan.Credit = planrequest.Credit;
            plan.CreditPrice = planrequest.CreditPrice;
            plan.PlanTypeId = planrequest.PlanTypeId;
            plan.NoOfAgents = planrequest.NoOfAgents;
            plan.NoOfDrivers = planrequest.NoOfDrivers;
            plan.NoOfVehicles = planrequest.NoOfVehicles;
            plan.PerCreditSMSPrice = planrequest.PerSMSPrice;
            plan.UpdatedDateTime = DateTime.Now;
            string date = planrequest.PlanExpiryDate.ToString();
            DateTime utcDate = DateTime.SpecifyKind(Convert.ToDateTime(date), DateTimeKind.Utc);
            plan.PlanExpiryDate = utcDate; //Convert.ToDateTime(date);
            plan = planService.Editplan(plan);
            return plan.Id;
        }

        private int PlanAlreadyExist(string planCode, int planTypeId)
        {
            using (DataContext context = new DataContext())
            {
                Plan planChk1 = context.plans.FirstOrDefault(x => x.PlanCode == planCode);
                Plan planChk2 = context.plans.FirstOrDefault(x => x.PlanTypeId == 1);
                

                
                if (planChk1 != null)
                    return -1001;
                if(planChk2 == null)
                    return 1;
                else if (planChk2.PlanTypeId == planTypeId)
                    return -1002;

                return 1;
            }

        }

        [HttpPost]
        public bool DeletePlan(DeletePlanRequest deleteplanrequest)
        {
            return planService.DeletePlan(deleteplanrequest.PlanId);
        }

    }



}
