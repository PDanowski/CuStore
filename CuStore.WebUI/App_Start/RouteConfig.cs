using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CuStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "",
                url: "Page_{pageNumber}",
                defaults: new
                {
                    Controller = "Product", action = "List",
                    category = (string)null, page = 1
                });

            routes.MapRoute(
                name: null,
                url: "Page_{pageNumber}",
                defaults: new
                {
                    Controller = "Product",
                    action = "List",
                    category = (string)null,
                    page = @"\d+"
                });

            routes.MapRoute(
                name: null,
                url: "{category}",
                defaults: new
                {
                    Controller = "Product",
                    action = "List",
                    category = (string)null,
                    page = 1
                });

            routes.MapRoute(
                name: null,
                url: "{category}/Page_{pageNumber}",
                defaults: new
                {
                    Controller = "Product",
                    action = "List"
                },
                constraints: new { page = @"\d+"});

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                defaults: new { controller = "Product", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}
