using CabicsSubscription.API.Models;
using CabicsSubscription.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CabicsSubscription.Service.Services;
using Braintree;
using System.Configuration;

namespace CabicsSubscription.API.Controllers
{
    public class SubscriptionController : ApiController
    {

        [HttpPost]
        public Service.Subscription InsertSubscription(InsertSubscriptionRequest insertSubscriptionRequest)
        {
            return null;
         
        }

        [HttpPost]
        public int InsertSubscriptionbyAdmin(InsertSubscriptionByAdminRequest insertSubscriptionByAdminRequest)
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

            if(chkautorenewel == true)
            {

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

            return subscriptionId;
        }

            public List<CreditDeductionType> GetAllCreditDeductionDetail()
        {
            SubscriptionService subscriptionService = new SubscriptionService();
            return subscriptionService.GetCreditDeductionDetail();

            
        }

        [HttpGet]
        public List<Service.Subscription> GetUserAllSubscriptionDetail(int cabOfficeId)
        {
            SubscriptionService subscriptionService = new SubscriptionService();
            List<Service.Subscription> lst = subscriptionService.GetUserAllSubscriptionDetail(cabOfficeId);

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

            List<Service.Subscription> finalLst = new List<Service.Subscription>();
            foreach (Service.Subscription subs in lst)
            {
               Transaction transaction = gateway.Transaction.Find(subs.btTransactionId);
                if (transaction.Status != Braintree.TransactionStatus.SETTLED  && transaction.Status != Braintree.TransactionStatus.SETTLEMENT_CONFIRMED)
                { subs.btTransactionId = "-1"; }

                finalLst.Add(subs);
            }

            return finalLst;

        }

        [HttpPost]
        public bool RefundSubscription(RefundRequest refundRequest)
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

           var refundResult = gateway.Transaction.Refund(refundRequest.TransactionId, Convert.ToDecimal(refundRequest.Amount));
           return refundResult.IsSuccess();

        }

        [HttpGet]
        public List<Service.Subscription> ShowCurrentSubscription(string userguid)
        {
            if (userguid != "undefined")
            {
                SubscriptionService subscriptionService = new SubscriptionService();
                return subscriptionService.ShowCurrentSubscription(userguid);
            }
            return null;
        }

        [HttpGet]
        public List<Service.Subscription> GetCredititUtilizationReport(int cabOfficeId, int subscriptionId)
        {
                SubscriptionService subscriptionService = new SubscriptionService();
                return subscriptionService.GetCredititUtilizationReport(cabOfficeId, subscriptionId);
        }

    }
}
