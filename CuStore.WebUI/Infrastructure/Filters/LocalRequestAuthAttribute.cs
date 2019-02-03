using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuStore.WebUI.Infrastructure.Filters
{
    public class LocalRequestAuthAttribute : AuthorizeAttribute
    {
        private readonly bool _localAllowed;

        public LocalRequestAuthAttribute(bool allowedParam)
        {
            _localAllowed = allowedParam;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.IsLocal)
            {
                return _localAllowed;
            }

            return true;
        }
    }
}