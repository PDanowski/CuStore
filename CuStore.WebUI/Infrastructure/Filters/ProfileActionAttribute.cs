using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuStore.WebUI.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ProfileActionAttribute : FilterAttribute, IActionFilter
    {
        private Stopwatch _timer;

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _timer = Stopwatch.StartNew();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result != null)
            {
                //logging here
                var message = $"Action {filterContext.ActionDescriptor.ActionName} time: {_timer.Elapsed.TotalSeconds}";
            }
        }
    }
}