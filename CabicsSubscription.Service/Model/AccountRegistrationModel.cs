using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CabicsSubscription.Service
{
    public class AccountRegistrationModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }
    }
}