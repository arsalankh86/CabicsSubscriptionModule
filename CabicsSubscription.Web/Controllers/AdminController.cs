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

        public ActionResult CreditUtilizationReport()
        {
            return View();
        }

        public ActionResult Dashboard()
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

        
        public ActionResult ExecuteService()
        {
            AutomatedService automatedService = new AutomatedService();
            automatedService.ExecutonFunction();

            return View();
        }


        [HttpPost]
        public ActionResult SubmitRefund(FormCollection form)
        {
            try
            {
                PlanService planService = new PlanService();

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
                Result<Transaction> refundResult = null;
                string transactionId = form["hdntransactionid"].ToString();
                Service.Subscription subscription = subscriptionService.GetSubscriptionByTransactionId(transactionId);
                int creditused = subscription.TotalCredit - subscription.RemainingCredit;
                double amountused = 0, remainingamount = 0, creditAmount = 0;

                if (form["hdnmode"].ToString() == "1")
                {
                    if (creditused != 0)
                    {
                        ViewBag.Error = "Unable to Fully Refund due to " + creditused + " credit(s) used ";
                        return View();
                    }

                    refundResult = gateway.Transaction.Refund(form["hdntransactionid"].ToString());
                    subscriptionService.DeactivateCurrentSubscription(transactionId);

                }
                else
                {
                    if (form["txtamount"] == "")
                    {
                        ViewBag.Error = "Please Enter Amount";
                        return View();
                    }

                    Service.Plan plan = planService.GetPlanDetailByPlanId(subscription.PlanId);
                    if (subscription.SubscriptionTypeId == 1)
                    {

                        creditAmount = Convert.ToDouble(plan.CreditPrice);
                        amountused = creditAmount * creditused;
                        remainingamount = subscription.SubscriptionPrice - amountused;
                        double newTotalPrice = subscription.TotalPrice - Convert.ToDouble(form["txtamount"]);
                        double totalCredit = newTotalPrice / creditAmount;
                        if (Convert.ToDouble(form["txtamount"]) > remainingamount)
                        {
                            ViewBag.Error = "Amount is greater than remaining amount ";
                            return View();
                        }
                        refundResult = gateway.Transaction.Refund(form["hdntransactionid"].ToString(), Convert.ToDecimal(form["txtamount"]));
                        if (refundResult.Message != "Cannot refund a transaction unless it is settled.")
                            subscriptionService.UpdateTotalCreditAndAmount(subscription.Id, newTotalPrice, totalCredit);
                    }
                    else if (subscription.SubscriptionTypeId == 2)
                    {
                        var daysInCurrentMonths = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                        var perDayCreditAmount = subscription.TotalPrice / daysInCurrentMonths;

                        var dayspend = DateTime.Now.Date.Day;

                        var amountUsed = dayspend * perDayCreditAmount;


                        if (Convert.ToDouble(form["txtamount"]) > amountUsed)
                        {
                            ViewBag.Error = "Amount is greater than remaining amount ";
                            return View();

                        }

                        var remainingAmount = subscription.TotalPrice - Convert.ToDouble(form["txtamount"]);
                        refundResult = gateway.Transaction.Refund(form["hdntransactionid"].ToString(), Convert.ToDecimal(form["txtamount"]));
                        if (refundResult.Message != "Cannot refund a transaction unless it is settled.")
                            subscriptionService.UpdateTotalCreditAndAmount(subscription.Id, remainingAmount, 0);
                    }

                }



                RefundTranactionLog refundTranactionLog = new RefundTranactionLog();
                refundTranactionLog.TransactionId = transactionId;
                refundTranactionLog.Message = refundResult.Message;
                if (refundResult.Errors != null)
                    refundTranactionLog.Errors = refundResult.Errors.ToString();
                refundTranactionLog.IsActive = true;
                refundTranactionLog.CreatedDateTime = DateTime.Now;
                refundTranactionLog.Gateway = Constant.Gateway.BrainTree.ToString();

                if (refundResult.Target != null)
                    refundTranactionLog.RefundTransactionId = refundResult.Target.RefundedTransactionId;


                subscriptionService.InsertRefundTranactionLog(refundTranactionLog);
            }
            catch(Exception ex)
            {
                throw ex;
            }
           

            return View();
        }


        [HttpPost]
        public int SubmitSubscription(FormCollection insertSubscriptionByAdminRequest)
        {
            try
            {
                double planamount = 0, smscreditamount = 0, amount = 0;

                if (insertSubscriptionByAdminRequest["hdnsmscreditamount"] != null && insertSubscriptionByAdminRequest["hdnsmscreditamount"] != "" && insertSubscriptionByAdminRequest["hdnsmscreditamount"] != "0")
                        smscreditamount = Convert.ToDouble(insertSubscriptionByAdminRequest["hdnsmscreditamount"]) * Convert.ToDouble(insertSubscriptionByAdminRequest["smscreditqty"]);

                amount = Convert.ToDouble(insertSubscriptionByAdminRequest["hdnamount"]);


                PlanService planService = new PlanService();
                int planId = 0;
                int accountid = 0;

                if (insertSubscriptionByAdminRequest["hdnplanid"] != "0")
                    planId = Convert.ToInt32(insertSubscriptionByAdminRequest["hdnplanid"]);

                CabicsSubscription.Service.Plan plan = planService.GetPlanDetailByPlanId(planId);

                if (insertSubscriptionByAdminRequest["ddlcaboofice"] != "0")
                    accountid = Convert.ToInt32(insertSubscriptionByAdminRequest["ddlcaboofice"]);

                bool resultt;
                Transaction transactionid;

                bool chkautorenewel = false;
                int noOfInstallment = 0;

                var bit = "off";
                if (insertSubscriptionByAdminRequest["chkautorenewel"] != null)
                    bit = insertSubscriptionByAdminRequest["chkautorenewel"].ToString();

                if (bit == "on")
                    chkautorenewel = true;


                //return RedirectToAction("Show", new { id = transaction.Id });
                SubscriptionService subscriptionService = new SubscriptionService();

                int qty = 1;
                if (insertSubscriptionByAdminRequest["qty"] != null)
                    qty = Convert.ToInt32(insertSubscriptionByAdminRequest["qty"]);

                int smscreditqty = 0;
                if (insertSubscriptionByAdminRequest["smscreditqty"] != "0")
                    smscreditqty = Convert.ToInt32(insertSubscriptionByAdminRequest["smscreditqty"]);

                double hdnsmscreditamount = 0;
                hdnsmscreditamount = Convert.ToInt32(insertSubscriptionByAdminRequest["hdnsmscreditotaltamount"]);


                int subscriptionId = subscriptionService.PurchaseSubscription(planId, amount,
                        accountid, qty, insertSubscriptionByAdminRequest["chequeNo"], smscreditqty, smscreditamount,
                       "adminsubscriptionbycheque", "", chkautorenewel, noOfInstallment);

                if (chkautorenewel == true)
                {

                    //// Insert into execution service

                    WindowsServiceExecution winservice = new WindowsServiceExecution();
                    winservice.WindowsServiceFunction = "Automatic Charging";
                    winservice.WindowsServiceArgumrnt = subscriptionId;
                    winservice.WindowsServiceFunctionCode = (int)Constant.WindowsFunction.AutomaticCharging;
                    winservice.WindowsServiceStatus = (int)Constant.WindowsServiceExecutionStatus.Pending;
                    winservice.IsActive = true;
                    winservice.CreatedDate = DateTime.UtcNow;

                    WindowsServiceExecutionService windowsServiceExecutionService = new WindowsServiceExecutionService();
                    windowsServiceExecutionService.InsertWindowsServiceExecutionService(winservice);


                }

                return subscriptionId;
            }
            catch (Exception ex)
            {
                DbErrorLogService.LogError("error", "InsertSubscriptionbyAdmin", "", ex.ToString());

                return 0;
            }
        }


        [HttpPost]
        public int InsertSubscriptionbyAdmin(Models.InsertSubscriptionByAdminRequest insertSubscriptionByAdminRequest)
        {
            try
            {
                double planamount = 0, smscreditamount = 0, amount = 0;

                if (insertSubscriptionByAdminRequest.hdnsmscreditamount != null && insertSubscriptionByAdminRequest.hdnsmscreditamount != 0)
                    smscreditamount = Convert.ToDouble(insertSubscriptionByAdminRequest.hdnsmscreditamount) * insertSubscriptionByAdminRequest.smscreditqty;

                amount = Convert.ToDouble(insertSubscriptionByAdminRequest.hdnamount);


                PlanService planService = new PlanService();
                int planId = 0;
                int accountid = 0;

                if (insertSubscriptionByAdminRequest.PlanId != 0)
                    planId = insertSubscriptionByAdminRequest.PlanId;

                CabicsSubscription.Service.Plan plan = planService.GetPlanDetailByPlanId(planId);

                if (insertSubscriptionByAdminRequest.cabofficeid != 0)
                    accountid = insertSubscriptionByAdminRequest.cabofficeid;

                bool resultt;
                Transaction transactionid;

                bool chkautorenewel = false;
                int noOfInstallment = 0;

                var bit = "off";
                if (insertSubscriptionByAdminRequest.chkautorenewel != null)
                    bit = insertSubscriptionByAdminRequest.chkautorenewel.ToString();

                if (bit == "on")
                    chkautorenewel = true;


                //return RedirectToAction("Show", new { id = transaction.Id });
                SubscriptionService subscriptionService = new SubscriptionService();

                int qty = 1;
                if (insertSubscriptionByAdminRequest.qty != null)
                    qty = Convert.ToInt32(insertSubscriptionByAdminRequest.qty);

                int smscreditqty = 0;
                if (insertSubscriptionByAdminRequest.smscreditqty != 0)
                    smscreditqty = insertSubscriptionByAdminRequest.smscreditqty;

                double hdnsmscreditamount = 0;
                hdnsmscreditamount = Convert.ToInt32(insertSubscriptionByAdminRequest.hdnsmscreditotaltamount);


                int subscriptionId = subscriptionService.PurchaseSubscription(planId, Convert.ToDouble(insertSubscriptionByAdminRequest.hdnamount),
                        insertSubscriptionByAdminRequest.cabofficeid, qty, insertSubscriptionByAdminRequest.chequeNo, smscreditqty, smscreditamount,
                       "adminsubscriptionbycheque", "", chkautorenewel, noOfInstallment);

                if (chkautorenewel == true)
                {

                    //// Insert into execution service

                    WindowsServiceExecution winservice = new WindowsServiceExecution();
                    winservice.WindowsServiceFunction = "Automatic Charging";
                    winservice.WindowsServiceArgumrnt = subscriptionId;
                    winservice.WindowsServiceFunctionCode = (int)Constant.WindowsFunction.AutomaticCharging;
                    winservice.WindowsServiceStatus = (int)Constant.WindowsServiceExecutionStatus.Pending;
                    winservice.IsActive = true;
                    winservice.CreatedDate = DateTime.UtcNow;

                    WindowsServiceExecutionService windowsServiceExecutionService = new WindowsServiceExecutionService();
                    windowsServiceExecutionService.InsertWindowsServiceExecutionService(winservice);


                }

                return subscriptionId;
            }
            catch (Exception ex)
            {
                DbErrorLogService.LogError("error", "InsertSubscriptionbyAdmin", "", ex.ToString());

                return 0;
            }
        }


    }

}