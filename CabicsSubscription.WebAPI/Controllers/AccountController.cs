using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CabicsSubscription.Service;
using CabicsSubscription.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CabicsSubscription.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


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