using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Braintree;
using CabicsSubscription.Service;
using CabicsSubscription.Service.Services;
using Hangfire;

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
           // AccountService accountService = new AccountService();
           // Account account = accountService.getCabOfficeByAccountId("sadasd");
            return View();
        }

        public ActionResult About()
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

            Result<PaymentMethodNonce> resultn = gateway.PaymentMethodNonce.Create("dxtcsh");
            String nonce2 = resultn.Target.Nonce;

            var requestp = new TransactionRequest
            {
                Amount = 20,
                PaymentMethodNonce = nonce2,
                //Options = new TransactionOptionsRequest
                //{
                //    SubmitForSettlement = true
                //}
            };

            Result<Transaction> resultp = gateway.Transaction.Sale(requestp);

           var resultt = resultp.IsSuccess();
            var transactionid = resultp.Target;

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

            amount = planamount + smscreditamount;

       

            #region Fetching CabOfficeAccount

            var accountid = "";
            if (form["hdnaccount"] != null)
                accountid = form["hdnaccount"].ToString();
            AccountService accountService = new AccountService();
            Account account = accountService.getCabOfficeByAccountId(accountid);

            if (account == null)
                return View();

            #endregion

            #region Payment Initilizer
            bool saleResult;

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
            #endregion

            #region Generate BTCustomer, token and Nonce
            String nonce_Generated = "";
            if (account.BtCustomerId == null || account.BtCustomerId == "")
            {
                /// Create Customer
                /// 
                var requestc = new CustomerRequest
                {
                    FirstName = account.FullName,
                    LastName = account.FullName,
                    Company = "",
                    Email = account.Email,
                    Fax = "",
                    Phone = "",
                    Website = ""
                };
                Result<Customer> resultc = gateway.Customer.Create(requestc);
                string customerId = resultc.Target.Id;


                /// Create PaymentMethod
                /// 
                var nonce = Request["payment_method_nonce"];
                var requestP = new PaymentMethodRequest
                {
                    CustomerId = customerId,
                    PaymentMethodNonce = nonce
                };

                Result<PaymentMethod> resultP = gateway.PaymentMethod.Create(requestP);
                Result<PaymentMethodNonce> resultN = gateway.PaymentMethodNonce.Create(resultP.Target.Token);
                nonce_Generated = resultN.Target.Nonce;

                /// Update BtCustoemrId and BtToken in CabOfficeAccount
                /// 
                accountService.UpdateBrainTreeInfo(customerId, resultP.Target.Token, account.Id);

            }
            else
            {
                Result<PaymentMethodNonce> resultN = gateway.PaymentMethodNonce.Create(account.BtPaymentMethodToken);
                nonce_Generated = resultN.Target.Nonce;
            }

            #endregion
            
            #region Sale

            var request = new TransactionRequest
            {
                Amount = amount,
                PaymentMethodNonce = nonce_Generated,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);

            bool resultVal = result.IsSuccess();
            Transaction transaction = result.Target;

            #endregion


            if (resultVal)
            {

                bool chkautorenewel = false;
                var bit = "off";
                if (form["chkautorenewel"] != null)
                    bit = form["chkautorenewel"].ToString();

                if (bit == "on")
                    chkautorenewel = true;

                SubscriptionService subscriptionService = new SubscriptionService();

                int qty = 1;
                if (form["qty"] != null && form["qty"] != "")
                    qty = Convert.ToInt32(form["qty"]);

                int smscreditqty = 0;
                if (form["smscreditqty"] != null && form["smscreditqty"] != "")
                    smscreditqty = Convert.ToInt32(form["smscreditqty"]);

                double hdnsmscreditamount = 0;
                if (form["hdnsmscreditamount"] != null && form["hdnsmscreditamount"] != "")
                    hdnsmscreditamount = Convert.ToDouble(form["hdnsmscreditamount"]);

                int accountId = 1;
                if (account != null)
                    accountId = account.Id;

                int planId = 0;
                if (form["hdnplanid"] != null)
                    planId = Convert.ToInt32(form["hdnplanid"].ToString());


                int subscriptionId = subscriptionService.PurchaseSubscription(planId, Convert.ToDouble(amount),
                    accountId, qty, "", smscreditqty, hdnsmscreditamount,
                    transaction.Id, "", chkautorenewel, 0);


                if (chkautorenewel == true)
                {


                    /// Mark Hangfire Service
                    AutomatedService automatedService = new AutomatedService();
                    RecurringJob.AddOrUpdate(() => automatedService.MarkAutoRenewalSubscription(subscriptionId, null), Cron.Hourly);


                    ////// Insert into execution service

                    WindowsServiceExecution winservice = new WindowsServiceExecution();
                    winservice.WindowsServiceFunction = "Automatic Charging";
                    winservice.WindowsServiceArgumrnt = subscriptionId;
                    winservice.WindowsServiceFunctionCode = (int)Constant.WindowsFunction.AutomaticCharging;
                    winservice.WindowsServiceStatus = (int)Constant.WindowsServiceExecutionStatus.Pending;
                    winservice.IsActive = true;
                    winservice.CreatedDate = DateTime.Now;

                    //WindowsServiceExecutionService windowsServiceExecutionService = new WindowsServiceExecutionService();
                    //windowsServiceExecutionService.InsertWindowsServiceExecutionService(winservice);
                }


                return RedirectToAction("Thankyou");
            }
            else
            {
                return RedirectToAction("CustomError?type=btpayment");
            }
        }


        public ActionResult PaymentResponse()
        {
            ViewBag.Result = TempData["Flash"];
            return View();
        }


        public ActionResult CustomError()
        {
            string errorType = "";
            if (Request.QueryString["type"] != null)
            {
                errorType = Request.QueryString["type"].ToString();
            }

            if (errorType == "btpayment")
                ViewBag.Error = "Your braintree payment transaction is unsuccesfull.";


            return View();
        }

    }
 }