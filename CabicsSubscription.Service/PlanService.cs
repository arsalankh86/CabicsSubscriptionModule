using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class PlanService
    {
        DataContext context = new DataContext();
        public List<Plan> GetAllPlan()
        {
            List<Plan> lstplan = context.plans.Where(x => x.IsActive == true).ToList();
            return lstplan;

        }

        public Plan GetPlanDetail(int planid)
        {
            Plan plan = context.plans.FirstOrDefault(x => x.IsActive == true && x.Id == planid);
            return plan;
        }
    }
}
