using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ItemsMVCWebApp.App_Start;
using ItemsMVCWebApp.Filters;

namespace ItemsMVCWebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            UnityWebApiConfig.Register(config);
            // Web API routes
            config.MapHttpAttributeRoutes();

            //Error and Exception Handling
            config.Filters.Add(new GlobalExceptionFilter());
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
