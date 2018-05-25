using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service.Services
{
    public class ConfigurationService
    {

        public int AddOrUpdateTextLocalConfiguration(TextlocalConfiguration textlocalConfiguration)
        {
            using (DataContext context = new DataContext())
            {
                TextlocalConfiguration config = context.TextlocalConfigurations.FirstOrDefault(x => x.CabofficeId == textlocalConfiguration.CabofficeId && x.IsActive == true);

                if (config == null)
                {
                    textlocalConfiguration.CreatedDate = DateTime.Now;
                    textlocalConfiguration.IsActive = true;
                    context.TextlocalConfigurations.Add(textlocalConfiguration);
                }
                else
                {
                    config.Password = textlocalConfiguration.Password;
                    config.Username = textlocalConfiguration.Username;
                    config.APIKey = textlocalConfiguration.APIKey;
                    config.Hash = textlocalConfiguration.Hash;
                }

                context.SaveChanges();

                return textlocalConfiguration.Id;

            }


        }

        public TextlocalConfiguration GetCabOfficeTextlocalConfiguration(string cabofficetoken)
        {
            using (DataContext context = new DataContext())
            {
                TextlocalConfiguration textlocalConfiguration = (from txtlocal in context.TextlocalConfigurations
                                join account in context.Accounts on txtlocal.CabofficeId equals account.Id
                                where account.IsActive == true && account.Token == cabofficetoken
                                select txtlocal).FirstOrDefault();

                return textlocalConfiguration;
            }
        }
    }
}
