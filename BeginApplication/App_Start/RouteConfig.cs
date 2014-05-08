using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BeginApplication
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null,
              "{controller}/{action}/Page{page}",
              new { controller = "Admin", action = "GetUsers" },
              new { page = @"\d+" }
              );

            routes.MapRoute(null,
              "{Controller}/{action}/{id}/Page{page}",
              new { controller = "Forum", action = "Section" },
              new { page = @"\d+" }
              );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );            
        }
    }
}