using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Braintree;
using CabicsSubscription.Service;
using CabicsSubscription.Service.Services;

namespace CabicsSubscription.Web.Controllers
{
    public class AdminController : Controller
    {
        SubscriptionService subscriptionService = new SubscriptionService();


        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddPlan()
        {
            return View();
        }

        public ActionResult EditPlan()
        {
            return View();
        }
        public ActionResult ViewPlan()
        {
            return View();
        }

        public ActionResult TextLocalConfiguration()
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

        public ActionResult ViewAllCabOffice()
        {
            return View();
        }

        public ActionResult ViewUserSubscription()
        {
            return View();
        }

        public ActionResult RefundRequest()
        {
            ViewBag.Error = "";
            return View();
        }


        [HttpPost]
        public ActionResult SubmitRefund(FormCollection form)
        {


            Braintree.Environment environment;
            if (ConfigurationManager.AppSettings["BtEnvironmentTestMode"].ToString() == "1")
                environment = Braintree.Environment.SANDBOX;
            else
                environment = Braintree.Environment.PRODUCTION;


            var gateway = new BraintreeGateway
            {
                Environment = environment,
                MerchantId = ConfigurationManager.AppSettings["BtMerchantId"],
                PublicKey = ConfigurationManager.AppSettings["BtPublicKey"],
                PrivateKey = ConfigurationManager.AppSettings["BtPrivateKey"]
            };
            Result<Transaction> refundResult;
            string transactionId = form["hdntransactionid"].ToString();

            if (form["hdnmode"].ToString() == "1")
            {
              refundResult = gateway.Transaction.Refund(form["hdntransactionid"].ToString());

                subscriptionService.DeleteSubscription(transactionId);

            
                
            }
            else
            {
                if (form["txtamount"] == "")
                {
                    ViewBag.Error = "Please Enter Amount";
                    return View();
                }

                refundResult = gateway.Transaction.Refund(form["hdntransactionid"].ToString(), Convert.ToDecimal(form["txtamount"]));
            }

            RefundTranactionLog refundTranactionLog = new RefundTranactionLog();
            refundTranactionLog.TransactionId = transactionId;
            refundTranactionLog.Message = refundResult.Message;
            refundTranactionLog.Errors = refundResult.Errors.ToString();
            refundTranactionLog.IsActive = true;
            refundTranactionLog.CreatedDateTime = DateTime.Now;
            refundTranactionLog.Gateway = Constant.Gateway.BrainTree.ToString();

            if(refundResult.Target != null)
                refundTranactionLog.RefundTransactionId = refundResult.Target.RefundedTransactionId;

            
            subscriptionService.InsertRefundTranactionLog(refundTranactionLog);

           

            return View();
        }
    }
}