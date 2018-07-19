using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service.Services
{
    public class AccountService
    {
        public string ReturnToken(AccountRegistrationModel accountRegistrationModel)
        {
            using (DataContext context = new DataContext())
            {

                Account account = GetAccountByEmail(accountRegistrationModel.Email);

                if (account == null)
                {
                    account = new Account();
                    string guid = Guid.NewGuid().ToString();
                    account.Email = accountRegistrationModel.Email;
                    account.FullName = accountRegistrationModel.Name;
                    account.ClientId = accountRegistrationModel.ClientId;
                    account.CabicsSystemId = accountRegistrationModel.CabicsSystemId;
                    account.Password = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);
                    account.AllCredit = 0;
                    account.BalanceCredit = 0;
                    account.CreatedDateTime = DateTime.Now;
                    account.IsActive = true;
                    account.Token = guid;
                    context.Accounts.Add(account);
                    context.SaveChanges();

                    return guid;
                }

                return account.Token;
            }

        }

        public Account getCabOfficeByToken(string token)
        {
            using (DataContext context = new DataContext())
            {
                Account account = context.Accounts.FirstOrDefault(x => x.IsActive == true && x.Token == token);
                return account;
            }
        }

        public List<Account> GetAllCabOffice()
        {
            using (DataContext contect = new DataContext())
            {
                List<Account> lstccount = contect.Accounts.Where(x => x.IsActive == true).ToList();
                return lstccount;
            }
        }

        public string GetAllCabToken(string email, int cabOfficeId)
        {
            using (DataContext contect = new DataContext())
            {
                Account account = contect.Accounts.FirstOrDefault(x => x.Email == email
                && x.CabicsSystemId == cabOfficeId
                && x.IsActive == true );

                if (account == null)
                    return Constant.APIError.AccountNotFound.ToString();

                return account.Token;
            }
        }

        private Account GetAccountByEmail(string email)
        {
            using (DataContext context = new DataContext())
            {
                Account account = context.Accounts.FirstOrDefault(x => x.Email == email && x.IsActive == true);
                return account;
            }

        }

        public bool UpdateActiveSubsctionForAccount(int subscriptionId, int cabofficeid)
        {
            try
            {
                using (DataContext context = new DataContext())
                {
                    Account account = context.Accounts.FirstOrDefault(x => x.Id == cabofficeid && x.IsActive == true);
                    account.CurrentSubscriptionId = subscriptionId;
                    context.SaveChanges();

                    List<Subscription> lstSubscription = context.Subscriptions.Where(x => x.AccountId == cabofficeid && x.Id != subscriptionId).ToList();
                    foreach(Subscription subscription in lstSubscription)
                    {
                        subscription.IsActive = false;
                        context.SaveChanges();
                    }




                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public Account getCabOfficeByAccountId(string accountid)
        {
            using (DataContext context = new DataContext())
            {
                Account account = context.Accounts.FirstOrDefault(x => x.Token == accountid  && x.IsActive == true);
                return account;
            }
        }

        public Account getCabOfficeByCabOfficeId(int cabOfficeId)
        {
            using (DataContext context = new DataContext())
            {
                Account account = context.Accounts.FirstOrDefault(x => x.Id == cabOfficeId && x.IsActive == true);
                return account;
            }
        }


        public void UpdatePaymentNonceInCabOfficeAccount(int cabOfficeId, string nonce)
        {
            using (DataContext context = new DataContext())
            {
                Account account = context.Accounts.FirstOrDefault(x => x.Id == cabOfficeId && x.IsActive == true);
                account.PaymentMethodNonce = nonce;
                context.SaveChanges();
            }

        }

        public void UpdateBrainTreeInfo(string btCustomerId, string btToken, int cabOfficeId)
        {
            using (DataContext context = new DataContext())
            {
                Account account = context.Accounts.FirstOrDefault(x => x.Id == cabOfficeId && x.IsActive == true);
                account.BtPaymentMethodToken = btToken;
                account.BtCustomerId = btCustomerId;
                context.SaveChanges();
            }
        }
    }

}
