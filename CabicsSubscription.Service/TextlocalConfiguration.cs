using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class TextlocalConfiguration
    {
        [Key]
        public int Id { get; set; }
        public int CabofficeId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string APIKey { get; set; }
        public string Hash { get; set; }       
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

    }
}
