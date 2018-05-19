using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class RefundTranactionLog
    {
        [Key]
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public string Message { get; set; }
        public string RefundTransactionId { get; set; }
        public string Gateway { get; set; }
        public string TransactionId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsActive { get; set; }
        public string Errors { get; set; }
    }
}
