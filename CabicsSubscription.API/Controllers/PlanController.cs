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
        public Plan InsertPlan(InsertPlanRequest planrequest)
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
            plan.PerSMSPrice = planrequest.PerSMSPrice;
            plan.IsActive = true;
            plan.CreatedDateTime = DateTime.Now;
            plan.PlanExpiryDate = Convert.ToDateTime(planrequest.PlanExpiryDate);


            return planService.InsertPlan(plan);
        }


        [HttpPost]
        public bool DeletePlan(DeletePlanRequest deleteplanrequest)
        {
            return planService.DeletePlan(deleteplanrequest.PlanId);
        }

    }



}
