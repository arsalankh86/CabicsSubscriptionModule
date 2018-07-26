using CabicsSubscription.API.Models;
using CabicsSubscription.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CabicsSubscription.Service.Services;
using System.Globalization;
using System.Web.Http.Description;
using System.Threading.Tasks;

namespace CabicsSubscription.API.Controllers
{
    public class UtilizationController : ApiController
    {

        
        AccountService accountService = new AccountService();
        SubscriptionService subscriptionService = new SubscriptionService();

        [HttpPost]
        [ResponseType(typeof(UtilizeSubscriptionDto))]
        //public UtilizeSubscriptionResponse UtilizeSubscription(UtilizeSubscriptionModel utilizeSubscriptionModel)
        public async Task<IHttpActionResult> UtilizeSubscription(UtilizeSubscriptionModel utilizeSubscriptionModel)
        {
            UtilizeSubscriptionDto utilizeSubscriptionResponse = new UtilizeSubscriptionDto();

            try
            {
                //AccountService accountService = new AccountService();
                //SubscriptionService subscriptionService = new SubscriptionService();

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
                    utilizeSubscriptionResponse.Result = false;
                    utilizeSubscriptionResponse.IsSuccess = true;
                    utilizeSubscriptionResponse.Error = Constant.APIError.AccountNotFound.ToString();
                    utilizeSubscriptionResponse.ErrorCode = (int)Constant.APIError.AccountNotFound;
                    return Ok(utilizeSubscriptionResponse);
                }

                Subscription subscription = subscriptionService.GetSubscriptionBySubscriptionId(Convert.ToInt32(account.CurrentSubscriptionId));

                if (subscription == null)
                {
                    utilizeSubscriptionResponse.Result = false;
                    utilizeSubscriptionResponse.IsSuccess = true;
                    utilizeSubscriptionResponse.Error = Constant.APIError.NoSubscriptionFound.ToString();
                    utilizeSubscriptionResponse.ErrorCode = (int)Constant.APIError.NoSubscriptionFound;
                    return Ok(utilizeSubscriptionResponse);
                }

                int subscriptionType = subscription.SubscriptionTypeId;

                if (subscriptionType == (int)Constant.SubscriptionType.Monthly)
                {
                    if (subscription.EndDate < DateTime.Now)
                    {
                        utilizeSubscriptionResponse.Result = false;
                        utilizeSubscriptionResponse.IsSuccess = true;
                        utilizeSubscriptionResponse.Error = Constant.APIError.SubscriptionExpired.ToString();
                        utilizeSubscriptionResponse.ErrorCode = (int)Constant.APIError.SubscriptionExpired;
                        return Ok(utilizeSubscriptionResponse);
                    }

                    if (utilizeSubscriptionModel.CreditUtilizationType == (int)Constant.CreditDeductionType.SMSCharges)
                    {

                        if (subscription.RemainingSmsCreditPurchase <= 0)
                        {
                            utilizeSubscriptionResponse.Result = false;
                            utilizeSubscriptionResponse.IsSuccess = true;
                            utilizeSubscriptionResponse.Error = Constant.APIError.NotEnoughMonthlySMSCredit.ToString();
                            utilizeSubscriptionResponse.ErrorCode = (int)Constant.APIError.NotEnoughMonthlySMSCredit;
                            return Ok(utilizeSubscriptionResponse);
                        }

                        CreditDeductionType smsCreditDeduction = subscriptionService.GetCreditSMSDeductionDetail();
                        subscriptionService.UpdateSubscriptionRemainingMonthlySMSCredit(subscription.Id, smsCreditDeduction.Credit, (int)Constant.CreditDeductionType.SMSCharges);
                    }

                }
                else
                {
                    if (subscription.RemainingCredit <= 0)
                    {
                        utilizeSubscriptionResponse.Result = false;
                        utilizeSubscriptionResponse.IsSuccess = true;
                        utilizeSubscriptionResponse.Error = Constant.APIError.NotEnoughCredit.ToString();
                        utilizeSubscriptionResponse.ErrorCode = (int)Constant.APIError.NotEnoughCredit;
                        return Ok(utilizeSubscriptionResponse);
                    }

                    if (utilizeSubscriptionModel.CreditUtilizationType == (int)Constant.CreditDeductionType.PerJobCharges)
                    {
                        CreditDeductionType jobCreditDeduction = subscriptionService.GetCreditJobDeductionDetail();
                        subscriptionService.UpdateSubscriptionCredit(subscription.Id, jobCreditDeduction.Credit, (int)Constant.CreditDeductionType.PerJobCharges);

                    }
                    else if (utilizeSubscriptionModel.CreditUtilizationType == (int)Constant.CreditDeductionType.SMSCharges)
                    {
                        CreditDeductionType smsCreditDeduction = subscriptionService.GetCreditSMSDeductionDetail();
                        subscriptionService.UpdateSubscriptionCredit(subscription.Id, smsCreditDeduction.Credit, (int)Constant.CreditDeductionType.SMSCharges);
                    }
                }



                utilizeSubscriptionResponse.Error = "";
                utilizeSubscriptionResponse.ErrorCode = 0;
                utilizeSubscriptionResponse.IsSuccess = true;
                utilizeSubscriptionResponse.Result = true;
                return Ok(utilizeSubscriptionResponse);

            }
            catch(Exception ex)
            {
                utilizeSubscriptionResponse.Error = ex.ToString();
                utilizeSubscriptionResponse.ErrorCode = (int)Constant.APIError.Exception;
                utilizeSubscriptionResponse.IsSuccess = false;
                utilizeSubscriptionResponse.Result = false;
                return Ok(utilizeSubscriptionResponse);
            }
        }


        [HttpGet]
        [ResponseType(typeof(SubscriptionStatusDto))]
        public async Task<IHttpActionResult> CheckSubscriptionStatus(int cabOfficeId, string cabOfficeEmail)
        {
            SubscriptionStatusDto subscriptionStatusDto = new SubscriptionStatusDto();
            try
            {
                
                Account account = accountService.getCabOfficeByEmailAndCabOfficeId(cabOfficeEmail, cabOfficeId);
                if (account == null)
                {
                    subscriptionStatusDto.isAllow = false;
                    subscriptionStatusDto.IsSuccess = true;
                    subscriptionStatusDto.Error = Constant.APIError.AccountNotFound.ToString();
                    subscriptionStatusDto.ErrorCode = (int)Constant.APIError.AccountNotFound;
                    return Ok(subscriptionStatusDto);
                }


                Subscription subscription = subscriptionService.GetSubscriptionBySubscriptionId(Convert.ToInt32(account.CurrentSubscriptionId));

                if (subscription == null)
                {
                    subscriptionStatusDto.isAllow = false;
                    subscriptionStatusDto.IsSuccess = true;
                    subscriptionStatusDto.Error = Constant.APIError.NoSubscriptionFound.ToString();
                    subscriptionStatusDto.ErrorCode = (int)Constant.APIError.NoSubscriptionFound;
                    return Ok(subscriptionStatusDto);
                }

                int subscriptionType = subscription.SubscriptionTypeId;


                if (subscriptionType == (int)Constant.SubscriptionType.Monthly)
                {
                    if (subscription.EndDate < DateTime.Now)
                    {
                        subscriptionStatusDto.isAllow = false;
                        subscriptionStatusDto.IsSuccess = true;
                        subscriptionStatusDto.Error = Constant.APIError.SubscriptionExpired.ToString();
                        subscriptionStatusDto.ErrorCode = (int)Constant.APIError.SubscriptionExpired;
                        return Ok(subscriptionStatusDto);
                    }
                }

                if (subscription.RemainingCredit <= 0)
                {
                        subscriptionStatusDto.isAllow = false;
                        subscriptionStatusDto.IsSuccess = true;
                        subscriptionStatusDto.Error = Constant.APIError.NotEnoughCredit.ToString();
                        subscriptionStatusDto.ErrorCode = (int)Constant.APIError.NotEnoughCredit;
                        return Ok(subscriptionStatusDto);
                }


                subscriptionStatusDto.isAllow = true;
                subscriptionStatusDto.IsSuccess = true;
                subscriptionStatusDto.Error = "";
                subscriptionStatusDto.ErrorCode = 0;
                return Ok(subscriptionStatusDto);

            }
            catch(Exception ex)
            {
                subscriptionStatusDto.isAllow = false;
                subscriptionStatusDto.IsSuccess = false;
                subscriptionStatusDto.Error = ex.ToString();
                subscriptionStatusDto.ErrorCode = (int)Constant.APIError.Exception;
                return Ok(subscriptionStatusDto);

            }





        }



        [HttpPost]
        [ResponseType(typeof(UpdateMonthlyQuotaDto))]
        public async Task<IHttpActionResult> UpdateMonthlyQuota(UpdateMonthlyQuotaModel updateMonthlyQuotaModel)
        {
            UpdateMonthlyQuotaDto updateMonthlyQuotaDto = new UpdateMonthlyQuotaDto();
            try
            {
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
                    updateMonthlyQuotaDto.Result = false;
                    updateMonthlyQuotaDto.IsSuccess = true;
                    updateMonthlyQuotaDto.Error = Constant.APIError.AccountNotFound.ToString();
                    updateMonthlyQuotaDto.ErrorCode = (int)Constant.APIError.AccountNotFound;
                    return Ok(updateMonthlyQuotaDto);
                }

                Subscription subscription = subscriptionService.GetSubscriptionBySubscriptionId(Convert.ToInt32(account.CurrentSubscriptionId));

                if (subscription == null)
                {
                    updateMonthlyQuotaDto.Result = false;
                    updateMonthlyQuotaDto.IsSuccess = true;
                    updateMonthlyQuotaDto.Error = Constant.APIError.NoSubscriptionFound.ToString();
                    updateMonthlyQuotaDto.ErrorCode = (int)Constant.APIError.NoSubscriptionFound;
                    return Ok(updateMonthlyQuotaDto);
                }

                if(subscription.SubscriptionTypeId != (int)Constant.SubscriptionType.Monthly)
                {
                    updateMonthlyQuotaDto.Result = false;
                    updateMonthlyQuotaDto.IsSuccess = false;
                    updateMonthlyQuotaDto.Error = Constant.APIError.WorngSubscriptionException.ToString();
                    updateMonthlyQuotaDto.ErrorCode = (int)Constant.APIError.WorngSubscriptionException;
                    return Ok(updateMonthlyQuotaDto);
                }
                
                if(updateMonthlyQuotaModel.QuotaType == (int)Constant.MonthlyQuotaType.Drivers)
                {
                    if(subscription.RemainingNoOfDrivers > 0)
                        subscriptionService.MinusDriverFromSubscriptionofCabOffice(subscription.Id);
                    else
                    {
                        updateMonthlyQuotaDto.Result = false;
                        updateMonthlyQuotaDto.IsSuccess = true;
                        updateMonthlyQuotaDto.Error = Constant.APIError.NotEnoughNoOfDriverExist.ToString();
                        updateMonthlyQuotaDto.ErrorCode = (int)Constant.APIError.NotEnoughNoOfDriverExist;
                        return Ok(updateMonthlyQuotaDto);
                    }

                }

                if (updateMonthlyQuotaModel.QuotaType == (int)Constant.MonthlyQuotaType.Agents)
                {
                    if (subscription.RemainingNoOfAgents > 0)
                        subscriptionService.MinusAgentFromSubscriptionofCabOffice(subscription.Id);
                    else
                    {
                        updateMonthlyQuotaDto.Result = false;
                        updateMonthlyQuotaDto.IsSuccess = true;
                        updateMonthlyQuotaDto.Error = Constant.APIError.NotEnoughNoOfAgentExist.ToString();
                        updateMonthlyQuotaDto.ErrorCode = (int)Constant.APIError.NotEnoughNoOfAgentExist;
                        return Ok(updateMonthlyQuotaDto);
                    }

                }

                if (updateMonthlyQuotaModel.QuotaType == (int)Constant.MonthlyQuotaType.Vehicles)
                {
                    if (subscription.RemainingNoOfVehicles > 0)
                        subscriptionService.MinusVehicleFromSubscriptionofCabOffice(subscription.Id);
                    else
                    {
                        updateMonthlyQuotaDto.Result = false;
                        updateMonthlyQuotaDto.IsSuccess = true;
                        updateMonthlyQuotaDto.Error = Constant.APIError.NotEnoughNoOfVehicleExist.ToString();
                        updateMonthlyQuotaDto.ErrorCode = (int)Constant.APIError.NotEnoughNoOfVehicleExist;
                        return Ok(updateMonthlyQuotaDto);
                    }

                }


                updateMonthlyQuotaDto.Result = true;
                updateMonthlyQuotaDto.IsSuccess = true;
                updateMonthlyQuotaDto.Error = "";
                updateMonthlyQuotaDto.ErrorCode = 0;
                return Ok(updateMonthlyQuotaDto);

            }
            catch (Exception ex)
            {
                updateMonthlyQuotaDto.Result = false;
                updateMonthlyQuotaDto.IsSuccess = false;
                updateMonthlyQuotaDto.Error = ex.ToString();
                updateMonthlyQuotaDto.ErrorCode = (int)Constant.APIError.Exception;
                return Ok(updateMonthlyQuotaDto);

            }


        }


        [HttpGet]
        [ResponseType(typeof(MonthlyQuotaDto))]
        public async Task<IHttpActionResult> CheckMonthlyQuotaStatus(int cabOfficeId, string cabOfficeEmail)
        {
            MonthlyQuotaDto monthlyQuotaDto = new MonthlyQuotaDto();
            try
            {

                Account account = accountService.getCabOfficeByEmailAndCabOfficeId(cabOfficeEmail, cabOfficeId);
                if (account == null)
                {
                    monthlyQuotaDto.QuoteDetailResponse = null; 
                    monthlyQuotaDto.IsSuccess = true;
                    monthlyQuotaDto.Error = Constant.APIError.AccountNotFound.ToString();
                    monthlyQuotaDto.ErrorCode = (int)Constant.APIError.AccountNotFound;
                    return Ok(monthlyQuotaDto);
                }


                Subscription subscription = subscriptionService.GetSubscriptionBySubscriptionId(Convert.ToInt32(account.CurrentSubscriptionId));

                if (subscription == null)
                {
                    monthlyQuotaDto.QuoteDetailResponse = null;
                    monthlyQuotaDto.IsSuccess = true;
                    monthlyQuotaDto.Error = Constant.APIError.NoSubscriptionFound.ToString();
                    monthlyQuotaDto.ErrorCode = (int)Constant.APIError.NoSubscriptionFound;
                    return Ok(monthlyQuotaDto);
                }

                if (subscription.SubscriptionTypeId != (int)Constant.SubscriptionType.Monthly)
                {
                    monthlyQuotaDto.QuoteDetailResponse = null;
                    monthlyQuotaDto.IsSuccess = false;
                    monthlyQuotaDto.Error = Constant.APIError.WorngSubscriptionException.ToString();
                    monthlyQuotaDto.ErrorCode = (int)Constant.APIError.WorngSubscriptionException;
                    return Ok(monthlyQuotaDto);
                }

                QuoteDetail quoteDetail = new QuoteDetail();
                quoteDetail.RemainingNoOfAgent = Convert.ToInt32(subscription.RemainingNoOfAgents);
                quoteDetail.RemainingNoOfDriver = Convert.ToInt32(subscription.RemainingNoOfDrivers);
                quoteDetail.RemainingNoOfVehicle = Convert.ToInt32(subscription.RemainingNoOfVehicles);

                monthlyQuotaDto.QuoteDetailResponse = quoteDetail;
                monthlyQuotaDto.IsSuccess = true;
                monthlyQuotaDto.Error = "";
                monthlyQuotaDto.ErrorCode = 0;
                return Ok(monthlyQuotaDto);

            }
            catch (Exception ex)
            {
                monthlyQuotaDto.QuoteDetailResponse = null;
                monthlyQuotaDto.IsSuccess = false;
                monthlyQuotaDto.Error = ex.ToString();
                monthlyQuotaDto.ErrorCode = (int)Constant.APIError.Exception;
                return Ok(monthlyQuotaDto);

            }


        }

    }
}
