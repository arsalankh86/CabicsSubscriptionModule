using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CabicsSubscription.Web.Models
{
    public class AccountRegistrationModelRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }


    }
}