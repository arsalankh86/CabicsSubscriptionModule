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
    public class PlanController : ApiController
    {
        PlanService planService = new PlanService();
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
                plan.IsActive = true;
                plan.CreatedDateTime = DateTime.Now;
                plan.UpdatedDateTime = DateTime.Now;
                // plan.PlanExpiryDate = DateTime.Now.AddMonths(6); 
                plan.PlanExpiryDate = Convert.ToDateTime(planrequest.PlanExpiryDate.ToString());
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
            // plan.PlanExpiryDate = DateTime.Now.AddMonths(6); 
            plan.PlanExpiryDate = Convert.ToDateTime(planrequest.PlanExpiryDate.ToString());
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
                else if (planChk2 != null)
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
