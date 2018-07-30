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
using System.Web.Http.Description;
using System.Threading.Tasks;

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

        [HttpPost]
        [ResponseType(typeof(TextLocalConfigurationDto))]
        public async Task<IHttpActionResult> GetCabOfficeTextlocalConfiguration(TextLocalConfigurationModel textLocalConfigurationModel)
        {
            TextLocalConfigurationDto textLocalConfigurationDto = new TextLocalConfigurationDto();
            ConfigurationService configurationService = new ConfigurationService();


            try
            {
                AccountService accountService = new AccountService();

                Account account = null;
                string email = "";
                int cabOfficeId = 0;
                if (Request.Headers.Contains("CabOfficeEmail"))
                {
                    IEnumerable<string> headerValues = Request.Headers.GetValues("CabOfficeEmail");
                    email = headerValues.FirstOrDefault();
                }

                if (Request.Headers.Contains("CabOfficeId"))
                {
                    IEnumerable<string> headerValues = Request.Headers.GetValues("CabOfficeId");
                    cabOfficeId = Convert.ToInt32(headerValues.FirstOrDefault());
                }

                account = accountService.getCabOfficeByEmailAndCabOfficeId(email, cabOfficeId);

               


                if (account == null)
                {
                    textLocalConfigurationDto.IsSuccess = true;
                    textLocalConfigurationDto.Error = Constant.APIError.AccountNotFound.ToString();
                    textLocalConfigurationDto.ErrorCode = (int)Constant.APIError.AccountNotFound;
                    return Ok(textLocalConfigurationDto);
                }

                TextlocalConfiguration textlocalConfiguration = configurationService.GetCabOfficeTextlocalConfiguration(account.Token);

                TextLocalConfigurationDetail textLocalConfigurationDetail = new TextLocalConfigurationDetail();
                textLocalConfigurationDetail.TextLocalAPIKey = textlocalConfiguration.APIKey;
                textLocalConfigurationDetail.TextLocalHash = textlocalConfiguration.Hash;
                textLocalConfigurationDetail.TextLocalPassword = textlocalConfiguration.Password;
                textLocalConfigurationDetail.TextLocalUsername = textlocalConfiguration.Username;

                textLocalConfigurationDto.textLocalConfiguration = textLocalConfigurationDetail;
                textLocalConfigurationDto.IsSuccess = true;
                textLocalConfigurationDto.Error = "";
                textLocalConfigurationDto.ErrorCode = 0;
                return Ok(textLocalConfigurationDto);


            }
            catch (Exception ex)
            {
                textLocalConfigurationDto.Error = ex.ToString();
                textLocalConfigurationDto.ErrorCode = (int)Constant.APIError.Exception;
                textLocalConfigurationDto.IsSuccess = false;
                return Ok(textLocalConfigurationDto);
            }
        }




    }
}
