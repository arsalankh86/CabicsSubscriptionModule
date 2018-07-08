using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service.Services
{
    public class PlanService
    {
        DataContext context = new DataContext();
        public List<Plan> GetAllPlan()
        {
            List<Plan> lstplan = context.plans.Where(x => x.IsActive == true).ToList();
            return lstplan;

        }

        public List<Plan> GetAllPlanForCabOffice()
        {

            List<Plan> lstplan = context.plans.Where(x => x.IsActive == true).ToList();
            //List<Plan> plan = new List<Plan>();
            //foreach (Plan p in lstplan)
            //{
            //    DateTime PlanExpiryDate = Convert.ToDateTime(p.PlanExpiryDateString.Replace('/','-'));
            //    if (PlanExpiryDate >= DateTime.Now)
            //    {
            //        plan.Add(p);
            //    }

            //}

            //return plan;

            return lstplan;

        }

        public Plan GetPlanDetail(int planid)
        {
            Plan plan = context.plans.FirstOrDefault(x => x.IsActive == true && x.Id == planid);
            return plan;
        }

        public Plan GetPlanDetailByPlanCode(string planCode)
        {
            Plan plan = context.plans.FirstOrDefault(x => x.IsActive == true && x.PlanCode == planCode);
            return plan;
        }

        public Plan InsertPlan(Plan plan)
        {
            try { 

            context.plans.Add(plan);
            context.SaveChanges();
            return plan;

                }
            catch(Exception ex)
            { return null; }
        }

        public bool DeletePlan(int planId)
        {
            try
            {
                Plan plan = context.plans.FirstOrDefault(x => x.Id == planId);
                plan.IsActive = false;
                context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }


        }

        public Plan Editplan(Plan planrequest)
        {
            using (DataContext context = new DataContext())
            {
                Plan plan = context.plans.FirstOrDefault(x => x.IsActive == true && x.PlanCode == planrequest.PlanCode);
                plan.Name = planrequest.Name;
                plan.Description = planrequest.Description;
                plan.CreditPrice = planrequest.CreditPrice;
                plan.NoOfAgents = planrequest.NoOfAgents;
                plan.NoOfDrivers = planrequest.NoOfDrivers;
                plan.NoOfVehicles = planrequest.NoOfVehicles;
                plan.PerCreditSMSPrice = planrequest.PerCreditSMSPrice;
                plan.UpdatedDateTime = DateTime.Now;
                // plan.PlanExpiryDate = DateTime.Now.AddMonths(6); 
                plan.PlanExpiryDate = Convert.ToDateTime(planrequest.PlanExpiryDate.ToString());
                context.SaveChanges();
                return plan;

            }
        }

        public Plan GetPlanDetailByPlanId(int planId)
        {
            Plan plan = context.plans.FirstOrDefault(x => x.Id == planId && x.IsActive == true);
            return plan;
        }
    }
}
