﻿using CabicsSubscription.API.Models;
using CabicsSubscription.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CabicsSubscription.API.Controllers
{
    public class PlanController : ApiController
    {
        PlanService planService = new PlanService();
        public List<Plan> GetAllPlan()
        {
           return planService.GetAllPlan();
        }
    }
}
