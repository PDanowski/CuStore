using System.Web;
using System.Web.Mvc;
using CuStore.WebUI.Infrastructure.Filters;

namespace Filters
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //only for Release
            //filters.Add(new UnhadledExceptionAttribute(null));
            filters.Add(new HandleErrorAttribute());
            
        }
    }
}
