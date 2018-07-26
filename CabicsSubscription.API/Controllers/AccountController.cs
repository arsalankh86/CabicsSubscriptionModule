
using CabicsSubscription.API.Models;
using CabicsSubscription.Service;
using CabicsSubscription.Service.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CabicsSubscription.API.Controllers
{
    public class AccountController : ApiController
    {

        [HttpPost]
        public string GetAccountToken(AccountRegistrationModelRequest accountRegistrationModelRequest)
        {
            AccountService accountService = new AccountService();

            AccountRegistrationModel accountRegistrationModel = new AccountRegistrationModel();
            accountRegistrationModel.Name = accountRegistrationModelRequest.Name;
            accountRegistrationModel.Email = accountRegistrationModelRequest.Email;
            accountRegistrationModel.ClientId = accountRegistrationModelRequest.ClientId;
            accountRegistrationModel.CabicsSystemId = accountRegistrationModelRequest.CabicsSystemId;
            return accountService.ReturnToken(accountRegistrationModel);


        }

        [HttpGet]
        public List<Account> GetCabOffice()
        {
            AccountService accountService = new AccountService();
            List<Account> lstAccount = accountService.GetAllCabOffice();
            return lstAccount;
        }

        [HttpGet]
        public Account GetCabOffice(string token)
        {
            AccountService accountService = new AccountService();
            Account account = accountService.getCabOfficeByToken(token);
            return account;
        }


        [HttpPost]
        public string Login(CabOfficeAuth cabOfficeAuth)
        {
            AccountService accountService = new AccountService();
            return accountService.GetAllCabToken(cabOfficeAuth.Email, cabOfficeAuth.CabOfficeId);
        }
    }
}
