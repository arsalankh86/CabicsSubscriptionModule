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
    public class ConfigurationController : ApiController
    {

        ConfigurationService configurationService = new ConfigurationService();


        [HttpPost]
        public int AddOrUpdateTextLocalConfiguration(TextlocalConfiguration textLocalConfiguration)
        {
            return configurationService.AddOrUpdateTextLocalConfiguration(textLocalConfiguration);
         
        }

        [HttpGet]
        public TextlocalConfiguration GetCabOfficeTextlocalConfiguration(string cabofficetoken)
        {
            return configurationService.GetCabOfficeTextlocalConfiguration(cabofficetoken);

        }

    }
}
