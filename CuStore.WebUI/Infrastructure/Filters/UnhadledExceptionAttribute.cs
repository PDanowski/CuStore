using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuStore.WebUI.Infrastructure.Filters
{
    public class UnhadledExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            filterContext.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary<string>(filterContext.Exception.Message)
            };
            filterContext.ExceptionHandled = true;
            //some logging here
        }
    }
}