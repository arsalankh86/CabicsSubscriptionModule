using Braintree;
using CabicsSubscription.Service;
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
                TempData["Flash"] = "Error: 81503: Amount is an invalid format.";
                return RedirectToAction("New");
            }

            var nonce = Request["payment_method_nonce"];
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
            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;
                //return RedirectToAction("Show", new { id = transaction.Id });
                SubscriptionService subscriptionService = new SubscriptionService();
                subscriptionService.PurchaseSubscription(planId, Convert.ToDouble(form["hdnamount"]), account.Id, Convert.ToInt32(form["qty"].ToString()), "");
            }
            else if (result.Transaction != null)
            {
                //return RedirectToAction("Show", new { id = result.Transaction.Id });
            }
            else
            {
                string errorMessages = "";
                foreach (ValidationError error in result.Errors.DeepAll())
                {
                    errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                }
                TempData["Flash"] = errorMessages;
                return RedirectToAction("New");
            }

            return View();


        }


    }
    }