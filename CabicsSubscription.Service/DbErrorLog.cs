using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class DbErrorLog
    {
        [Key]
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string ModuleFunction { get; set; }
        public string FunctionArgument { get; set; }
        public string Error { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
