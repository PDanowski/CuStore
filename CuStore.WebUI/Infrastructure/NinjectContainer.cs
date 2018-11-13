using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Ninject.Modules;

namespace CuStore.WebUI.Infrastructure
{
    public class NinjectContainer
    {
        private static NinjectResolver _resolver;

        /// <summary>
        /// Sets up the IOC using the Ninject Modules provided
        /// </summary>
        /// <param name="modules">Modules</param>
        public static void RegisterModules(params NinjectModule[] modules)
        {
            _resolver = new NinjectResolver(modules);
            DependencyResolver.SetResolver(_resolver);
        }

        /// <summary>
        /// Sets up the IOC Container loading modules from the Currently Executing Assembly
        /// </summary>
        public static void RegisterAssembly()
        {
            _resolver = new NinjectResolver(Assembly.GetExecutingAssembly());

            //This is where the actual hookup to the MVC Pipeline is done.
            DependencyResolver.SetResolver(_resolver);
        }

        /// <summary>
        /// Manually Resolve Dependencies 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return _resolver.Kernel.Get<T>();
        }
    }
}