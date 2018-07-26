using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CabicsSubscription.API.Models
{
    public class DTO
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public int ErrorCode { get; set; }
    }
}