using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class WindowsServiceExecution
    {
        [Key]
        public int Id { get; set; }
        public int WindowsServiceFunctionCode { get; set; }
        public string WindowsServiceFunction { get; set; }
        public int WindowsServiceArgumrnt { get; set; }
        public int WindowsServiceStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
