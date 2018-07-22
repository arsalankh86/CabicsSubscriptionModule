using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string AccountCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int CabicsSystemId { get; set; }
        public Nullable<int> BalanceCredit { get; set; }
        public Nullable<int> AllCredit { get; set; }
        public Nullable<int> CurrentSubscriptionId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; }
      
        //Foreign key for Client
        public int ClientId { get; set; }
        public string PaymentMethodNonce { get; set; }
        public Client client { get; set; }

        public string BtCustomerId { get; set; }
        public string BtPaymentMethodToken { get; set; }


    }
}
