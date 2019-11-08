using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Castle.Facilities.WcfIntegration;
using Castle.Windsor;

namespace CuStore.CRMService
{
    public class Global : System.Web.HttpApplication
    {

        static IWindsorContainer container;

        protected void Application_Start(Object sender, EventArgs e)
        {
            container = new WindsorContainer();
            container.AddFacility<WcfFacility>();
            container.Install(new WindsorInstaller()); //The class you just created.
        }

    }
}