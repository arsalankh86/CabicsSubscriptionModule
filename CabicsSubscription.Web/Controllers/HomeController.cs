using Braintree;
using CabicsSubscription.Service;
using CabicsSubscription.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CabicsSubscription.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult TextLocalConfiguration()
        {
            return View();
        }

        public ActionResult ViewPlan()
        {
            return View();
        }

        public ActionResult PurchaseSubscription()
        {
            int plancode = 0;
            string acccountguid = "";

            if (Request.QueryString["id"] != null)
                plancode = Convert.ToInt32(Request.QueryString["id"].ToString());
            if (Request.QueryString["account"] != null)
                acccountguid = Request.QueryString["account"].ToString();




            var gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "gbhg9d4dvt83v4ff",
                PublicKey = "n4yhd55nx6g2ygdn",
                PrivateKey = "374d896ef9682b3550a76d2b82811d1a"
            };

            var clientToken = gateway.ClientToken.Generate(
            //    new ClientTokenRequest
            //    {
            //        CustomerId = "1"
            //    }
            );


            ViewBag.ClientToken = clientToken;

            return View();
        }


        public ActionResult Dashboard()
        {
            AccountService accountService = new AccountService();
            Account account = accountService.getCabOfficeByAccountId("sadasd");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Thankyou()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {

            var gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "gbhg9d4dvt83v4ff",
                PublicKey = "n4yhd55nx6g2ygdn",
                PrivateKey = "374d896ef9682b3550a76d2b82811d1a"
            };

            var clientToken = gateway.ClientToken.Generate(
            //    new ClientTokenRequest
            //    {
            //        CustomerId = "1"
            //    }
            );


            ViewBag.ClientToken = clientToken;


            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult SubmitContact(FormCollection form)
        {
            Decimal planamount = 0, smscreditamount = 0, amount = 0;
           
            if (form["hdnamount"] != null && form["hdnamount"]!= "")
                planamount = Convert.ToDecimal(form["hdnamount"]);
            if(form["hdnsmscreditotaltamount"] != null && form["hdnsmscreditotaltamount"] != "")
                smscreditamount = Convert.ToDecimal(form["hdnsmscreditotaltamount"]);

            amount = planamount;// + smscreditamount;

            PlanService planService = new PlanService();
            var planId = 0;
            var accountid="";

            if (form["hdnplanid"] != null)
                planId = Convert.ToInt32(form["hdnplanid"].ToString());

           CabicsSubscription.Service.Plan plan = planService.GetPlanDetailByPlanId(planId);

            if (form["hdnaccount"] != null)
                accountid = form["hdnaccount"].ToString();

            AccountService accountService = new AccountService();
            Account account = accountService.getCabOfficeByAccountId(accountid);

            bool resultt;
            Transaction transactionid;

            var gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "gbhg9d4dvt83v4ff",
                PublicKey = "n4yhd55nx6g2ygdn",
                PrivateKey = "374d896ef9682b3550a76d2b82811d1a"
            };

            var nonce = Request["payment_method_nonce"];
            bool chkautorenewel = false;
            int noOfInstallment = 0;

            var bit = "off";
            if (form["chkautorenewel"] != null)
                bit = form["chkautorenewel"].ToString();

            if (bit == "on")
                chkautorenewel = true;

            if (chkautorenewel == false)
            {
                var request = new TransactionRequest
                {
                    Amount = amount,
                    PaymentMethodNonce = nonce,
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    }
                };

                Result<Transaction> result = gateway.Transaction.Sale(request);

                resultt = result.IsSuccess();
                transactionid = result.Target;

                var transaction = result.Target;

                //return RedirectToAction("Show", new { id = transaction.Id });
                SubscriptionService subscriptionService = new SubscriptionService();

                int qty = 1;
                if (form["qty"] != null)
                    qty = Convert.ToInt32(form["qty"]);

                int smscreditqty = 0;
                if (form["smscreditqty"] != "")
                    smscreditqty = Convert.ToInt32(form["smscreditqty"]);

                double hdnsmscreditamount = 0;
                if (form["hdnsmscreditamount"] != "")
                    hdnsmscreditamount = Convert.ToInt32(form["hdnsmscreditamount"]);

                int subscriptionId = subscriptionService.PurchaseSubscription(planId, Convert.ToDouble(form["hdnamount"]), account.Id, qty, "", smscreditqty, hdnsmscreditamount, 
                    transaction.Id,"", chkautorenewel, noOfInstallment);
            }
            else
            {
                if (form["noOfBillingCycle"] != null && form["noOfBillingCycle"] != "")
                    noOfInstallment = Convert.ToInt32(form["noOfBillingCycle"].ToString());


                var rrequest = new CustomerRequest
                {
                    FirstName = form["fname"],
                    LastName = form["lname"],
                    PaymentMethodNonce = nonce
                };
                Result<Customer> rresult = gateway.Customer.Create(rrequest);

                bool success = rresult.IsSuccess();
                // true

                Customer customer = rresult.Target;
                string customerId = customer.Id;
                // e.g. 160923

                string cardToken = customer.PaymentMethods[0].Token;
                // e.g. f28w


                var request = new SubscriptionRequest
                {
                    PaymentMethodToken = cardToken,
                    PlanId = plan.BrainTreePlanName
                    //,NumberOfBillingCycles = noOfInstallment
                };

                Result<Braintree.Subscription> result = gateway.Subscription.Create(request);
                resultt = result.IsSuccess();



                var transaction = result.Target;

                //return RedirectToAction("Show", new { id = transaction.Id });
                SubscriptionService subscriptionService = new SubscriptionService();

                bool chkAutoRenewel = false;
                string btSubscriptionId = result.Target.Id;

                int qty = 1;
                if (form["qty"] != null && form["qty"] != "")
                    qty = Convert.ToInt32(form["qty"]);

                int smscreditqty = 0;
                if (form["smscreditqty"]!= null && form["smscreditqty"] != "")
                    smscreditqty = Convert.ToInt32(form["smscreditqty"]);

                double hdnsmscreditamount = 0;
                if (form["hdnsmscreditamount"] != null && form["hdnsmscreditamount"] != "")
                    hdnsmscreditamount = Convert.ToInt32(form["hdnsmscreditamount"]);

                int subscriptionId =  subscriptionService.PurchaseSubscription(planId, Convert.ToDouble(amount), account.Id, qty, "", smscreditqty, hdnsmscreditamount,
                    transaction.Id, btSubscriptionId, chkautorenewel, noOfInstallment);

                //// Insert into execution service

                WindowsServiceExecution winservice = new WindowsServiceExecution();
                winservice.WindowsServiceFunction = "Automatic Charging";
                winservice.WindowsServiceArgumrnt = subscriptionId;
                winservice.WindowsServiceFunctionCode = (int)Constant.WindowsFunction.AutomaticCharging;
                winservice.WindowsServiceStatus = (int)Constant.WindowsServiceExecutionStatus.Pending;
                winservice.IsActive = true;
                winservice.CreatedDate = DateTime.Now;

                WindowsServiceExecutionService windowsServiceExecutionService = new WindowsServiceExecutionService();
                windowsServiceExecutionService.InsertWindowsServiceExecutionService(winservice);






            }

            
            if (resultt)
            {
                Transaction transaction = null;
                //return RedirectToAction("Show", new { id = transaction.Id });
                SubscriptionService subscriptionService = new SubscriptionService();

                int qty = 1;
                if (form["qty"]!= "" && form["qty"] != null)
                    qty = Convert.ToInt32(form["qty"]);

                int smscreditqty = 0;
                if (form["smscreditqty"]!= null && form["smscreditqty"] != "")
                    smscreditqty = Convert.ToInt32(form["smscreditqty"]);

                double hdnsmscreditamount = 0;
                if (form["hdnsmscreditamount"]!= null && form["hdnsmscreditamount"] != "")
                    hdnsmscreditamount = Convert.ToInt32(form["hdnsmscreditamount"]);

                //subscriptionService.PurchaseSubscription(planId, Convert.ToDouble(form["hdnamount"]), account.Id, qty, "", smscreditqty, hdnsmscreditamount, transaction.Id);
            }
            //else if (resultt.Transaction != null)
            //{
            //    //return RedirectToAction("Show", new { id = result.Transaction.Id });
            //}
            //else
            //{
            //    string errorMessages = "";
            //    foreach (ValidationError error in result.Errors.DeepAll())
            //    {
            //        errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
            //    }
            //    TempData["Flash"] = errorMessages;
            //    return RedirectToAction("PaymentResponse");
            //}
            return RedirectToAction("Thankyou");
            //return View();


        }


        public ActionResult PaymentResponse()
        {
            ViewBag.Result = TempData["Flash"];
            return View();
        }


    }
    }