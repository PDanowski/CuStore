using System.Collections.Generic;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.Domain.Repositories;
using CuStore.UnitTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CuStore.UnitTests.Repositories
{
    [TestClass]
    public class ProductRepositoryTests
    {
        [TestMethod]
        public void GetProductsCountByPhrase_FullCollection_ReturnCount()
        {
            Mock<IStoreContext> mock = new Mock<IStoreContext>();
            mock.Setup(m => m.Products).Returns(MockDbSet.GetMockDbSet<Product>(new List<Product>{
                new Product {Id = 1, Name = "ProductName", Price = 10, CategoryId = 3},
                new Product {Id = 2, Name = "ProductName2", Price = 20, CategoryId = 3},
                new Product {Id = 3, Name = "ProductName3", Price = 10, CategoryId = 4},
                new Product {Id = 4, Name = "Product4", Price = 25, CategoryId = 5},
                new Product {Id = 5, Name = "Product5", Price = 30, CategoryId = 6 }
            }).Object);

            IProductRepository repo = new ProductRepository(mock.Object);

            var result = repo.GetProductsCountByPhrase("ProductName");

            Assert.AreEqual(result, 3);
        }

        [TestMethod]
        public void GetProductsCountByPhrase_FullCollection_ReturnCountThoughDifferentCase()
        {
            Mock<IStoreContext> mock = new Mock<IStoreContext>();
            mock.Setup(m => m.Products).Returns(MockDbSet.GetMockDbSet<Product>(new List<Product>{
                new Product {Id = 1, Name = "ProductName", Price = 10, CategoryId = 3},
                new Product {Id = 2, Name = "ProductName2", Price = 20, CategoryId = 3},
                new Product {Id = 3, Name = "ProductName3", Price = 10, CategoryId = 4},
                new Product {Id = 4, Name = "Product4", Price = 25, CategoryId = 5},
                new Product {Id = 5, Name = "Product5", Price = 30, CategoryId = 6 }
            }).Object);

            IProductRepository repo = new ProductRepository(mock.Object);

            var result = repo.GetProductsCountByPhrase("productname");

            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void GetProductsCountByPhrase_FullCollection_ReturnCountComplexNames()
        {
            Mock<IStoreContext> mock = new Mock<IStoreContext>();
            mock.Setup(m => m.Products).Returns(MockDbSet.GetMockDbSet(new List<Product>{
                new Product {Id = 1, Name = "ProductNameIsEasy", Price = 10, CategoryId = 3},
                new Product {Id = 2, Name = "ProductName2345", Price = 20, CategoryId = 3},
                new Product {Id = 3, Name = "ProductName3", Price = 10, CategoryId = 4},
                new Product {Id = 4, Name = "Product4", Price = 25, CategoryId = 5},
                new Product {Id = 5, Name = "Product5", Price = 30, CategoryId = 6 }
            }).Object);

            IProductRepository repo = new ProductRepository(mock.Object);

            var result = repo.GetProductsCountByPhrase("Name");

            Assert.AreEqual(result, 3);
        }

        [TestMethod]
        public void GetProductsCountByPhrase_FullCollectionWithCategories_ReturnCountComplexNames()
        {
            Mock<IStoreContext> mock = new Mock<IStoreContext>();
            mock.Setup(m => m.Products).Returns(MockDbSet.GetMockDbSet(new List<Product>{
                new Product {Id = 1, Name = "ProductNameIsEasy", Price = 10, CategoryId = 3},
                new Product {Id = 2, Name = "ProductName2345", Price = 20, CategoryId = 3},
                new Product {Id = 3, Name = "ProductName3", Price = 10, CategoryId = 4},
                new Product {Id = 4, Name = "Product4", Price = 25, CategoryId = 5},
                new Product {Id = 5, Name = "Product5", Price = 30, CategoryId = 6 }
            }).Object);

            IProductRepository repo = new ProductRepository(mock.Object);

            var result = repo.GetProductsCountByPhrase("Name", 3);

            Assert.AreEqual(result, 2);
        }

        [TestMethod]
        public void GetProductsCountByPhrase_EmptyCollection_ReturnsNone()
        {
            Mock<IStoreContext> mock = new Mock<IStoreContext>();
            mock.Setup(m => m.Products).Returns(MockDbSet.GetMockDbSet(new List<Product>()).Object);

            IProductRepository repo = new ProductRepository(mock.Object);

            var result = repo.GetProductsCountByPhrase("ProductName");

            Assert.AreEqual(result, 0);
        }

    }
}

