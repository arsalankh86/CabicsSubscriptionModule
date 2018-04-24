using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;

namespace CabicsSubscription.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                    name: "DefualtAPI",
                    routeTemplate: "api/{controller}/{action}/{data}",
                    defaults: new { data = RouteParameter.Optional }
                );


            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);


        }



    }
}
