using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DataContext : DbContext
    {
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<CreditDeductionLog> CreditDeductionLogs { get; set; }
        public virtual DbSet<CreditDeductionType> CreditDeductionTypes { get; set; }
        public virtual DbSet<Plan> plans { get; set; }
        public virtual DbSet<PlanType> PlanType { get; set; }
        public virtual DbSet<SubscriptionStatus> SubscriptionStatus { get; set; }
        public virtual DbSet<Subscription> Subscription { get; set; }
        public virtual DbSet<SubscriptionType> SubscriptionType { get; set; }
        public virtual DbSet<RefundTranactionLog> RefundTranactionLogs { get; set; }
        public virtual DbSet<TranactionLog> TranactionLogs { get; set; }

        public virtual DbSet<TextlocalConfiguration> TextlocalConfigurations { get; set; }

        public DataContext() : base("CabicsSubscriptionDB")
        {

        }
    }
}
