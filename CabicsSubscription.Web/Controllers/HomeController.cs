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
            var planId = 0;
            var accountid="";

            if (form["hdnplanid"] != null)
                planId = Convert.ToInt32(form["hdnplanid"].ToString());

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

            Decimal amount;
            try
            {
                amount = Convert.ToDecimal(form["hdnamount"]);
            }
            catch (FormatException e)
            {
               // TempData["Flash"] = "Error: 81503: Amount is an invalid format.";
                return RedirectToAction("New");
            }

            var nonce = Request["payment_method_nonce"];
            var chkautorenewel = form["chkautorenewel"];
            if(chkautorenewel == "No")
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
            }
            else
            {
                var rrequest = new CustomerRequest
                {
                    FirstName = "Fred",
                    LastName = "Jones",
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
                    PlanId = "aa05"
                };

                Result<Braintree.Subscription> result = gateway.Subscription.Create(request);
                resultt = result.IsSuccess();
            }

            
            if (resultt)
            {
                Transaction transaction = null;
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

                subscriptionService.PurchaseSubscription(planId, Convert.ToDouble(form["hdnamount"]), account.Id, qty, "", smscreditqty, hdnsmscreditamount, transaction.Id);
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