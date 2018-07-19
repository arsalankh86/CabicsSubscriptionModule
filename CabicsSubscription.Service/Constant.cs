using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public static class Constant
    {
        public enum APIError
        {
            AccountNotFound  = 1001,
            NoSubscriptionFound = 2001,
            SubscriptionExpired = 2002,
            NotEnoughCredit = 2003
        }
        

        public enum SubscriptionStatus
        {
            
            Pending = 1,
            Active = 2,
            Expired = 3
        }

        public enum Gateway
        {

            BrainTree = 1,
            PayPal = 2,
        }



        public enum PlayType
        {

            PayAsYouGo = 1,
            Monthly = 2,
        }


        public enum SubscriptionType
        {
            PayAsYouGo = 1,
            Monthly = 2,
        }

        public enum WindowsServiceExecutionStatus
        {
            Pending = 1,
            Done = 2,
            Error = 3
        }


        public enum WindowsFunction
        {
            AutomaticCharging = 1
        }


    }
}
