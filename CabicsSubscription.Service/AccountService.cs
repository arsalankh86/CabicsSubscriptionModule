﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
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

        private Account GetAccountByEmail(string email)
        {
            using (DataContext context = new DataContext())
            {
                Account account = context.Accounts.FirstOrDefault(x => x.Email == email && x.IsActive == true);
                return account;
            }

        }
    }

}