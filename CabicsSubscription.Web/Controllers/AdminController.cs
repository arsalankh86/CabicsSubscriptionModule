using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CabicsSubscription.Web.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddPlan()
        {
            return View();
        }


        public ActionResult ViewPlan()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitPlan(FormCollection form)
        {
            return View();
        }

        public ActionResult AddSubscriptionByAdmin()
        {
            return View();
        }



    }
}