using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CabicsSubscription.API.Models
{
    public class TextLocalConfigurationDto :DTO
    {
        public TextLocalConfigurationDetail textLocalConfiguration { get; set; }
    }

    public class TextLocalConfigurationDetail
    {
        public string TextLocalUsername { get; set; }
        public string TextLocalPassword { get; set; }
        public string TextLocalAPIKey { get; set; }
        public string TextLocalHash { get; set; }
    }
 }