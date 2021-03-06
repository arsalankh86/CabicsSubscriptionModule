﻿using System;
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
                           name: "DefaultApi",
                           routeTemplate: "api/{controller}/{action}/{id}",
                           defaults: new { id = RouteParameter.Optional }
                       );

            var enableCorsAttribute = new EnableCorsAttribute("*",
                                            "Origin, Content-Type, Accept",
                                            "GET, PUT, POST, DELETE, OPTIONS");
            config.EnableCors(enableCorsAttribute);


        }



    }
}
