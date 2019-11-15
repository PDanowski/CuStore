using CuStore.Domain.Abstract;
using CuStore.Domain.Concrete;
using CuStore.Domain.Constants;
using CuStore.Domain.Repositories;
using CuStore.WebUI.Infrastructure.Abstract;
using CuStore.WebUI.Infrastructure.Helpers;
using CuStore.WebUI.Infrastructure.Implementations;
using Ninject.Modules;
using Ninject.Web.Common;

namespace CuStore.WebUI
{
    public class NinjectConfig : NinjectModule
    {
        public override void Load()
        {
            Bind<IStoreContext>().To<StoreContext>().InRequestScope();
            Bind<IProductRepository>().To<ProductRepository>().InRequestScope();
            Bind<ICategoryRepository>().To<CategoryRepository>().InRequestScope();
            Bind<IOrderRepository>().To<OrderRepository>().InRequestScope();
            Bind<ICartRepository>().To<CartRepository>().InRequestScope();
            Bind<IShippingMethodRepository>().To<ShippingMethodRepository>().InRequestScope();
            Bind<IUserRepository>().To<UserRepository>().InRequestScope();

            //Because we are using OnePerRequestHttpModule, the default behaviour is to create a new instance of the EmployeeContext 
            //for each Http request. That means different requests will never share an instance of a context. 
            //It will also ensure that no more than one StoreContext is created, 
            //even if the request ends up hitting 3 controllers that all require an StoreContext. 
            //In other words, the lifetime of the context is tied to the life of the request. 
            //This is a good thing and is definitely the recommended approach for Entity Framework. 

            EmailServerConfiguration config = new EmailServerConfiguration();
            Bind<IEmailSender>().To<EmailSender>().WithConstructorArgument("configuration", config);
            Bind<IPlacesApiClient>().To<GoogleMapsApiClient>().InRequestScope();
            Bind<ICountriesProvider>().To<CountriesProvider>().InSingletonScope();
            Bind<ILogger>().To<FileLogger>().InSingletonScope();
            Bind<ICrmClient>().To<CrmClient>().InRequestScope();
            Bind<ICrmClientAdapter>().To<CrmClientAdapter>().InRequestScope();
        }
    }
}