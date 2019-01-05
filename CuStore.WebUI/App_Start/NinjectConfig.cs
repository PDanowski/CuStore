using CuStore.Domain.Abstract;
using CuStore.Domain.Concrete;
using CuStore.Domain.Constants;
using Ninject.Modules;
using Ninject.Web.Common;

namespace CuStore.WebUI
{
    public class NinjectConfig : NinjectModule
    {
        public override void Load()
        {
            Bind<IStoreContext>().To<StoreContext>().InRequestScope();
            Bind<IStoreRepository>().To<StoreRepository>().InRequestScope();

            //Because we are using OnePerRequestHttpModule, the default behaviour is to create a new instance of the EmployeeContext 
            //for each Http request. That means different requests will never share an instance of a context. 
            //It will also ensure that no more than one StoreContext is created, 
            //even if the request ends up hitting 3 controllers that all require an StoreContext. 
            //In other words, the lifetime of the context is tied to the life of the request. 
            //This is a good thing and is definitely the recommended approach for Entity Framework. 

            EmailServerConfiguration config = new EmailServerConfiguration();
            Bind<IEmailSender>().To<EmailSender>().WithConstructorArgument("configuration", config);
        }
    }
}