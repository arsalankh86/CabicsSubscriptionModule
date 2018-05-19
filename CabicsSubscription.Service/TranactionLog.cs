using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class TranactionLog
    {
        [Key]
       public int Id { get; set; }
       public int SubscriptionId { get; set; }
        public string TransactionId { get; set; }
        public string Gateway { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsActive { get; set; }

    }
}
