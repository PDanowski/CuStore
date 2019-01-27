using System.Web.Mvc;

namespace CuStore.WebUI.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Admin";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Admin_default",
                url: "Admin",
                defaults: new { controller = "Manage", action = "Index" },
                namespaces: new[] { "CuStore.WebUI.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                name: "Admin",
                url: "Admin/{controller}/{action}",
                defaults: new { controller = "Manage", action = "Index" },
                constraints: new { controller = "Manage" },
                namespaces: new[] { "CuStore.WebUI.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                name: null,
                url: "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Manage", action = "Index", categoryId = UrlParameter.Optional },
                constraints: new { controller = "Manage" },
                namespaces: new[] { "CuStore.WebUI.Areas.Admin.Controllers" }
            );
        }
    }
}