using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace CuStore.WebUI.Infrastructure
{
    public class CustomRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new CustomHttpHandler();
        }

        public class CustomHttpHandler : IHttpHandler
        {
            public void ProcessRequest(HttpContext context)
            {
                Debug.WriteLine("User: " + context.User.Identity.Name);
                Debug.WriteLine("Request ContentType: " + context.Request.ContentType);
                Debug.WriteLine("Request UserAgent: " + context.Request.UserAgent);
                Debug.WriteLine("Request UserHostAddress: " + context.Request.UserHostAddress);
                Debug.WriteLine("Request UserHostName: " + context.Request.UserHostName);
                Debug.WriteLine("Request HttpMethod: " + context.Request.HttpMethod);
                Debug.WriteLine("Request RequestType: " + context.Request.RequestType);

                context.Response.Write("DEBUG RESPONSE" +
                                       "User: " + context.User.Identity.Name +
                                       "Request UserAgent: " + context.Request.UserAgent +
                                       "Request UserHostAddress: " + context.Request.UserHostAddress);
            }

            public bool IsReusable => false;
        }
    }
}