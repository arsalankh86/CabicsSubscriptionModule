using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public static class Constant
    {
        public enum SubscriptionStatus
        {
            
            Pending = 1,
            Active = 2,
            Expired = 3
        }

        public enum PlayType
        {

            PayAsYouGo = 1,
            Monthly = 2,
        }


        public enum SubscriptionType
        {

            Pending = 1,
            Active = 2,
            Expired = 3
        }


    }
}
