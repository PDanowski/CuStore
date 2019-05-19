using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuStore.WebUI.Infrastructure.Abstract;

namespace CuStore.WebUI.Infrastructure.Filters
{
    public class UnhadledExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        private readonly ILogger _logger;

        public UnhadledExceptionAttribute(ILogger logger)
        {
            this._logger = logger;
        }

        public void OnException(ExceptionContext filterContext)
        {
            _logger?.LogException(filterContext.Exception);

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