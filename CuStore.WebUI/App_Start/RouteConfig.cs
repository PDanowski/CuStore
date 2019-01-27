using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
using System.Web.Routing;
using CuStore.WebUI.Infrastructure;

namespace CuStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //route.DataTokens["UseNamespaceFallback"] = false;
            //To disable default namespaces searching fallback

            //Enable routing attributes in controllers (MVC 5)
            routes.MapMvcAttributeRoutes();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.Add(new Route("DebugRoute", new CustomRouteHandler()));

            routes.MapRoute(
                name: "",
                url: "Page_{pageNumber}",
                defaults: new
                {
                    controller = "Product", action = "List",
                    category = (string)null, page = 1
                },
                constraints: new { pageNumber = new RangeRouteConstraint(1, 1000), httpMethod = new HttpMethodConstraint("GET") },
                namespaces: new[] { "CuStore.WebUI.Controllers" });

            routes.MapRoute(
                name: null,
                url: "Page_{pageNumber}",
                defaults: new
                {
                    controller = "Product",
                    action = "List",
                    category = (string)null,
                    page = @"\d+"
                },
                constraints: new { pageNumber = @"^\d{1,4}$" },
                namespaces: new[] { "CuStore.WebUI.Controllers" });

            routes.MapRoute(
                name: null,
                url: "{category}",
                defaults: new
                {
                    controller = "Product",
                    action = "List",
                    category = (string)null,
                    page = 1
                },
                namespaces: new[] { "CuStore.WebUI.Controllers" });

            routes.MapRoute(
                name: null,
                url: "{category}/Page_{pageNumber}",
                defaults: new
                {
                    controller = "Product",
                    action = "List"
                },
                constraints: new { pageNumber = @"^\d{1,4}$", category 
                    = new CompoundRouteConstraint(new IRouteConstraint[]
                    {
                        new MinLengthRouteConstraint(1),
                        new MaxLengthRouteConstraint(300), 
                    } ) },
                namespaces: new[] { "CuStore.WebUI.Controllers" });

            routes.MapRoute(
                name: "List",
                url: "{controller}/{action}/{id}",
                //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                defaults: new { controller = "Product", action = "List", categoryId = UrlParameter.Optional },
                constraints: new { controller = "Product" },
                namespaces: new[] { "CuStore.WebUI.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Product", action = "List" },
                namespaces: new[] { "CuStore.WebUI.Controllers" }
            );
        }
    }
}
