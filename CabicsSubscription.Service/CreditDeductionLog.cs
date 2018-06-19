using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class CreditDeductionLog
    {
        [Key]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int subscriptionId { get; set; }
        public int CreditDeductionTypeId { get; set; }
        public int JobId { get; set; }
        public int Credits { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }

    }
}
