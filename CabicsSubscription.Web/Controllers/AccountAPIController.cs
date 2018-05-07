using CabicsSubscription.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CabicsSubscription.Service;
using CabicsSubscription.Service.Services;

namespace CabicsSubscription.Web.Controllers
{
    public class AccountAPIController : ApiController
    {
        AccountService accountService = new AccountService();

        [HttpPost]
        public string GetAccountToken(AccountRegistrationModelRequest accountRegistrationModelRequest)
        {

            AccountRegistrationModel accountRegistrationModel = new AccountRegistrationModel();
            accountRegistrationModel.Name = accountRegistrationModelRequest.Name;
            accountRegistrationModel.Email = accountRegistrationModelRequest.Email;
            accountRegistrationModel.ClientId = accountRegistrationModelRequest.ClientId;
            return accountService.ReturnToken(accountRegistrationModel);
            

        }
    }
}
