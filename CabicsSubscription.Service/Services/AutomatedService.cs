using Braintree;
using CabicsSubscription.Service.Services;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class AutomatedService
    {
        WindowsServiceExecutionService windowsServiceExecutionService = new WindowsServiceExecutionService();
        SubscriptionService subscriptionService = new SubscriptionService();
        AccountService accountService = new AccountService();

        public void ExecutonFunction()
        {
            List<WindowsServiceExecution> lst = windowsServiceExecutionService.GetAllPendingExecution();

            foreach (WindowsServiceExecution windowsServiceExecution in lst)
            {
                switch (windowsServiceExecution.WindowsServiceFunctionCode)
                {
                    case 1:
                        MarkAutoRenewalSubscription(windowsServiceExecution.WindowsServiceArgumrnt, windowsServiceExecution);
                        break;



                }



            }
        }

        public void MxecutonFunction()
        {
            DbErrorLog errorlog = new DbErrorLog();
            errorlog.FunctionArgument = "2";
            errorlog.Error = "Entered";
            errorlog.ModuleFunction = "Check";
            DataContext dataContext = new DataContext();
            dataContext.DbErrorLogs.Add(errorlog);
            dataContext.SaveChanges();
        }


        public void MarkAutoRenewalSubscription(int subscriptionId, WindowsServiceExecution windowsServiceExecution)
        {
            try
            {
                WindowsServiceLogging.WriteEventLog("******************* -- Event: MarkAutoRenewalSubscription( -- ***********************");
                

                WindowsServiceExecutionService windowsServiceExecutionService = new WindowsServiceExecutionService();
                //List<WindowsServiceExecution> lstSubscription = windowsServiceExecutionService.GetAutoRenewelEntryBySubscriptionID(subscriptionId);
                Subscription subscription = subscriptionService.GetSubscriptionBySubscriptionId(subscriptionId);

                if (subscription == null)
                    return;

                if (subscription.SubscriptionTypeId != (int)Constant.SubscriptionType.Monthly)
                    return;

                int cabOfficeId = subscription.AccountId;
                Account account = accountService.getCabOfficeByCabOfficeId(cabOfficeId);
                int subscriptionTypeId = subscription.SubscriptionTypeId;
                int planId = subscription.PlanId;
                double amount = Convert.ToDouble(subscription.TotalPrice);

                DateTime startDate = subscription.StartDate;
                string endDate = subscription.EndDate.ToString().Split(' ')[0] ;
                string todayDate = DateTime.Now.ToString().Split(' ')[0];

                var qty = 1;
                int smscreditqty = Convert.ToInt32(subscription.NoOfSmsCreditPurchase);
                double hdnsmscreditamount = Convert.ToInt32(subscription.SMSPrice);
                string transactionId = subscription.btTransactionId;


                WindowsServiceLogging.WriteEventLog("******************* -- Checking for Date Matched EndDate: " + endDate+ " Today Date: " + todayDate + " -- ***********************");
                if (endDate == todayDate)
                {



                    WindowsServiceLogging.WriteEventLog("******************* -- Date Matched -- ***********************");

                    /// Generate Monthly Sale

                    if (account.BtPaymentMethodToken == null || account.BtPaymentMethodToken == "")
                        return;

                    #region Payment 

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


                    Result<PaymentMethodNonce> resultN = gateway.PaymentMethodNonce.Create(account.BtPaymentMethodToken);
                    var nonce_Generated = resultN.Target.Nonce;

                    var request = new TransactionRequest
                    {
                        Amount = Convert.ToDecimal(amount),
                        PaymentMethodNonce = nonce_Generated,
                        Options = new TransactionOptionsRequest
                        {
                            SubmitForSettlement = true
                        }
                    };

                    Result<Transaction> result = gateway.Transaction.Sale(request);

                    bool isSuccess = result.IsSuccess();
                    Transaction transaction = result.Target;

                    #endregion

                    if (isSuccess)
                    {
                        //// Insert Re-Subscription
                    int nSubscriptionId = subscriptionService.PurchaseSubscription(planId, Convert.ToDouble(amount), account.Id, qty, "", smscreditqty, hdnsmscreditamount,
                           subscription.btTransactionId, subscription.btSubscriptionId, subscription.IsAutoRenewel, subscription.NoOfBillingCycle);


                        RecurringJob.AddOrUpdate(() => MarkAutoRenewalSubscription(nSubscriptionId, null), Cron.Hourly);

                        ////// Mark execution service Done
                        //windowsServiceExecutionService.MarkWindowsServiceStatus(windowsServiceExecution.Id, (int)Constant.WindowsServiceExecutionStatus.Done);

                        //// Insert into execution service
                        WindowsServiceExecution winservice = new WindowsServiceExecution();
                        winservice.WindowsServiceFunction = "Automatic Charging";
                        winservice.WindowsServiceArgumrnt = subscriptionId;
                        winservice.WindowsServiceFunctionCode = (int)Constant.WindowsFunction.AutomaticCharging;
                        winservice.WindowsServiceStatus = (int)Constant.WindowsServiceExecutionStatus.Pending;
                        winservice.IsActive = true;
                        winservice.CreatedDate = DateTime.Now;
                        windowsServiceExecutionService.InsertWindowsServiceExecutionService(winservice);
                    }
                }
            }
            catch(Exception ex)
            {
                DbErrorLogService.LogError("CabicsSubscriptionModule", "MarkAutoRenewalSubscription", "SubscriptionId: " + subscriptionId, ex.ToString());
            }


        }

        public void DeductDailyCredit(int subscriptionId)
        {
                SubscriptionService subscriptionService = new SubscriptionService();
                CreditDeductionType dailyCreditDeduction = subscriptionService.GetCreditDailyDeductionDetail();
                int dailyCreditDeductionCreditValue = dailyCreditDeduction.Credit;
                subscriptionService.UpdateSubscriptionRemainingMonthlySMSCredit(subscriptionId, dailyCreditDeduction.Credit);
        }
    }

    
}
