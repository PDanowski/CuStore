using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CuStore.CRMService.DAL;
using CuStore.CRMService.DAL.Abstract;
using CuStore.CRMService.DataContracts;
using CuStore.CRMService.Services;

namespace CuStore.CRMService
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ICrmService, CrmService>(),
                Component.For<ICustomerDataProvider, CustomerDataProvider>(),
                Component.For<ICrmContext, CrmContext>());
        }
    }
}