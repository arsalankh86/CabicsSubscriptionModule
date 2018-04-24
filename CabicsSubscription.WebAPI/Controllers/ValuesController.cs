using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CabicsSubscription.Service;
using CabicsSubscription.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CabicsSubscription.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPost]
        public string GetAccountToken(AccountRegistrationModelRequest accountRegistrationModelRequest)
        {
            AccountService accountService = new AccountService();
            AccountRegistrationModel accountRegistrationModel = new AccountRegistrationModel();
            accountRegistrationModel.Name = accountRegistrationModelRequest.Name;
            accountRegistrationModel.Email = accountRegistrationModelRequest.Email;
            accountRegistrationModel.ClientId = accountRegistrationModelRequest.ClientId;
            return accountService.ReturnToken(accountRegistrationModel);


        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
