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
using System.Web.Http.Description;
using System.Threading.Tasks;

namespace CabicsSubscription.API.Controllers
{
    public class UtilizationController : ApiController
    {
        PlanService planService = new PlanService();

        [HttpPost]
        [ResponseType(typeof(UtilizeSubscriptionDto))]
        //public UtilizeSubscriptionResponse UtilizeSubscription(UtilizeSubscriptionModel utilizeSubscriptionModel)
        public async Task<IHttpActionResult> UtilizeSubscription(UtilizeSubscriptionModel utilizeSubscriptionModel)
        {
            UtilizeSubscriptionDto utilizeSubscriptionResponse = new UtilizeSubscriptionDto();
            AccountService accountService = new AccountService();
            SubscriptionService subscriptionService = new SubscriptionService();
       
            Account account = null;
            string email = "";
            int cabOfficeId = 0;
            if (Request.Headers.Contains("CabOfficeEmail")) {
                IEnumerable<string> headerValues = Request.Headers.GetValues("CabOfficeEmail");
                email = headerValues.FirstOrDefault();
            }

            if (Request.Headers.Contains("CabOfficeId"))
            {
                IEnumerable<string> headerValues = Request.Headers.GetValues("CabOfficeId");
                cabOfficeId = Convert.ToInt32(headerValues.FirstOrDefault());
            }

            account =  accountService.getCabOfficeByEmailAndCabOfficeId(email, cabOfficeId);
            Subscription subscription = subscriptionService.GetSubscriptionBySubscriptionId(Convert.ToInt32(account.CurrentSubscriptionId));
            int subscriptionType = subscription.SubscriptionTypeId;


            if (account == null)
            {
                utilizeSubscriptionResponse.Response = "";
                utilizeSubscriptionResponse.Result = false;
                utilizeSubscriptionResponse.Error = Constant.APIError.AccountNotFound.ToString();
                utilizeSubscriptionResponse.ErrorCode = (int)Constant.APIError.AccountNotFound;
                return Ok(utilizeSubscriptionResponse);
            }

            if (subscription == null)
            {
                utilizeSubscriptionResponse.Response = "";
                utilizeSubscriptionResponse.Result = false;
                utilizeSubscriptionResponse.Error = Constant.APIError.NoSubscriptionFound.ToString();
                utilizeSubscriptionResponse.ErrorCode = (int)Constant.APIError.NoSubscriptionFound;
                return Ok(utilizeSubscriptionResponse);
            }

        

            if (subscriptionType == (int)Constant.SubscriptionType.Monthly)
            {
                if (subscription.EndDate < DateTime.Now)
                {
                    utilizeSubscriptionResponse.Response = "";
                    utilizeSubscriptionResponse.Result = false;
                    utilizeSubscriptionResponse.Error = Constant.APIError.SubscriptionExpired.ToString();
                    utilizeSubscriptionResponse.ErrorCode = (int)Constant.APIError.SubscriptionExpired;
                    return Ok(utilizeSubscriptionResponse);
                }

                if (utilizeSubscriptionModel.CreditUtilizationType == (int)Constant.CreditDeductionType.SMSCharges)
                {
                        
                    if (subscription.RemainingSmsCreditPurchase <= 0)
                    {
                        utilizeSubscriptionResponse.Response = "";
                        utilizeSubscriptionResponse.Result = false;
                        utilizeSubscriptionResponse.Error = Constant.APIError.NotEnoughMonthlySMSCredit.ToString();
                        utilizeSubscriptionResponse.ErrorCode = (int)Constant.APIError.NotEnoughMonthlySMSCredit;
                        return Ok(utilizeSubscriptionResponse);
                    }
                    CreditDeductionType smsCreditDeduction = subscriptionService.GetCreditSMSDeductionDetail();
                    subscriptionService.UpdateSubscriptionRemainingMonthlySMSCredit(subscription.Id, smsCreditDeduction.Credit);
                }

            }
            else
            {
                if (subscription.RemainingCredit <= 0)
                {
                    utilizeSubscriptionResponse.Response = "";
                    utilizeSubscriptionResponse.Result = false;
                    utilizeSubscriptionResponse.Error = Constant.APIError.NotEnoughCredit.ToString();
                    utilizeSubscriptionResponse.ErrorCode = (int)Constant.APIError.NotEnoughCredit;
                    return Ok(utilizeSubscriptionResponse);
                }

                if (utilizeSubscriptionModel.CreditUtilizationType == (int)Constant.CreditDeductionType.PerJobCharges)
                {
                    CreditDeductionType jobCreditDeduction = subscriptionService.GetCreditJobDeductionDetail();
                    subscriptionService.UpdateSubscriptionRemainingMonthlySMSCredit(subscription.Id, jobCreditDeduction.Credit);

                }
                else if (utilizeSubscriptionModel.CreditUtilizationType == (int)Constant.CreditDeductionType.SMSCharges)
                {
                    CreditDeductionType smsCreditDeduction = subscriptionService.GetCreditSMSDeductionDetail();
                    subscriptionService.UpdateSubscriptionRemainingMonthlySMSCredit(subscription.Id, smsCreditDeduction.Credit);
                }
            }

            CreditDeductionLog creditDeductionLog = new CreditDeductionLog();
            creditDeductionLog.AccountId = account.Id;
            creditDeductionLog.subscriptionId = subscription.Id;
            creditDeductionLog.CreditDeductionTypeId = utilizeSubscriptionModel.CreditUtilizationType;
            creditDeductionLog.CreatedDate = DateTime.UtcNow;

            utilizeSubscriptionResponse.Error = "";
            utilizeSubscriptionResponse.ErrorCode = 0;
            utilizeSubscriptionResponse.Result = true;
            utilizeSubscriptionResponse.Response = "";
            return Ok(utilizeSubscriptionResponse);

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
