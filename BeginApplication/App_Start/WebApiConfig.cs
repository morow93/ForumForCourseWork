using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BeginApplication
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Включает поддержку запросов с типом возвращаемого значения IQueryable или IQueryable<T>.
            config.EnableQuerySupport();
        }
    }
}