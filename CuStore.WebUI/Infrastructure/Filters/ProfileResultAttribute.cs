using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuStore.WebUI.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ProfileResultAttribute : FilterAttribute, IResultFilter
    {
        private Stopwatch _timer;

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            _timer = Stopwatch.StartNew();
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.Result != null)
            {
                //logging here
                var message = $"Processing result time: {_timer.Elapsed.TotalSeconds}";
            }
        }
    }
}