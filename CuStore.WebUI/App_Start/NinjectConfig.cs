using CuStore.Domain.Abstract;
using CuStore.Domain.Concrete;
using Ninject.Modules;

namespace CuStore.WebUI
{
    public class NinjectConfig : NinjectModule
    {
        public override void Load()
        {
            //Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            //mock.Setup(m => m.Products).Returns(new List<Product>
            //{
            //    new Product {Id = 1, Name = "Product1", Price = 10, CategoryId = 1},
            //    new Product {Id = 2, Name = "Product2", Price = 20, CategoryId = 2},
            //    new Product {Id = 3, Name = "Product3", Price = 30, CategoryId = 3}
            //});
            //mock.Setup(m => m.Categories).Returns(new List<Category>
            //{
            //    new Category {Id = 1, Name = "Category1"},
            //    new Category {Id = 2, Name = "Category2"},
            //    new Category {Id = 3, Name = "Category3"}
            //});

            //_kernel.Bind<IStoreRepository>().ToConstant(mock.Object);

            Bind<IStoreContext>().To<StoreContext>();
            Bind<IStoreRepository>().To<StoreRepository>();
        }
    }
}