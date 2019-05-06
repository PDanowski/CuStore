using System;
using System.Linq;
using System.Web.Mvc;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.WebUI.Controllers;
using CuStore.WebUI.HtmlHelpers;
using CuStore.WebUI.Models;
using CuStore.WebUI.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CuStore.UnitTests.Controllers
{
    [TestClass]
    public class ProductControllerTest
    {
        [TestMethod]
        public void ProductList_ValidCollection_CanPaginate()
        {
            int pageSize = 5;
            int pageNumber = 1;
            Mock<IProductRepository> mockProd = new Mock<IProductRepository>();
            Mock<ICategoryRepository> mockCat = new Mock<ICategoryRepository>();

            mockProd.Setup(m => m.GetProductsByCategory(pageSize, pageNumber, null)).Returns(new Product[]
            {
                new Product {Id = 1, Name = "Product1", Price = 10, CategoryId = 1},
                new Product {Id = 2, Name = "Product2", Price = 20, CategoryId = 2},
                new Product {Id = 3, Name = "Product3", Price = 30, CategoryId = 3},
                new Product {Id = 4, Name = "Product4", Price = 10, CategoryId = 1},
                new Product {Id = 5, Name = "Product5", Price = 20, CategoryId = 2}
            });

            ProductController controller = new ProductController(mockProd.Object, mockCat.Object);
            ProductsListViewModel result = (ProductsListViewModel) controller.List(null, pageNumber).Model;

            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 5);
            Assert.AreEqual(prodArray[0].Name, "Product1");
        }

        [TestMethod]
        public void ProductList_ValidCollection_PaginatationViewModelCorrect()
        {
            int pageSize = 5;
            int pageNumber = 2;
            Mock<IProductRepository> mockProd = new Mock<IProductRepository>();
            Mock<ICategoryRepository> mockCat = new Mock<ICategoryRepository>();

            mockProd.Setup(m => m.GetProductsByCategory(pageSize, pageNumber, null))
                .Returns(new Product[]
            {
                new Product {Id = 1, Name = "Product1", Price = 10, CategoryId = 2},
                new Product {Id = 2, Name = "Product2", Price = 20, CategoryId = 2},
                new Product {Id = 3, Name = "Product3", Price = 10, CategoryId = 3},
                new Product {Id = 4, Name = "Product4", Price = 20, CategoryId = 4},
                new Product {Id = 5, Name = "Product5", Price = 30, CategoryId = 5},
                new Product {Id = 6, Name = "Product6", Price = 10, CategoryId = 6},
                new Product {Id = 7, Name = "Product7", Price = 20, CategoryId = 7}
            });
            mockProd.Setup(m => m.GetProductsCount(null)).Returns(7);

            ProductController controller = new ProductController(mockProd.Object, mockCat.Object);
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, pageNumber).Model;

            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.ItemsPerPage, 5);
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.TotalPages, 2);
            Assert.AreEqual(pageInfo.TotalItems, 7);
        }

        [TestMethod]
        public void ProductList_PageNumberSet_ProductsFiltered()
        {
            int pageSize = 5;
            int pageNumber = 1;
            Mock<IProductRepository> mockProd = new Mock<IProductRepository>();
            Mock<ICategoryRepository> mockCat = new Mock<ICategoryRepository>();

            Category cat1 = new Category
            {
                Id = 1,
                Name = "Cat1"
            };
            Category cat2 = new Category
            {
                Id = 2,
                Name = "Cat2"
            };

            mockProd.Setup(m => m.GetProductsByCategory(pageSize, pageNumber, null))
                .Returns(new Product[]
                {
                    new Product {Id = 2, Name = "Product2", Price = 20, CategoryId = 2, Category = cat2},
                    new Product {Id = 3, Name = "Product3", Price = 20, CategoryId = 2, Category = cat2},
                    new Product {Id = 4, Name = "Product4", Price = 20, CategoryId = 1, Category = cat1},
                    new Product {Id = 5, Name = "Product5", Price = 30, CategoryId = 2, Category = cat2},
                    new Product {Id = 6, Name = "Product6", Price = 10, CategoryId = 2, Category = cat2},
                });

            mockProd.Setup(m => m.GetProductsByCategory(pageSize, pageNumber, 1))
                .Returns(new Product[]
                {
                    new Product {Id = 4, Name = "Product4", Price = 20, CategoryId = 1, Category = cat1},
                    new Product {Id = 7, Name = "Product7", Price = 20, CategoryId = 1, Category = cat1}
                });

            mockProd.Setup(m => m.GetProductsByCategory(pageSize, pageNumber, 2))
                .Returns(new Product[]
                {
                    new Product {Id = 2, Name = "Product2", Price = 20, CategoryId = 2, Category = cat2},
                    new Product {Id = 3, Name = "Product3", Price = 20, CategoryId = 2, Category = cat2},
                    new Product {Id = 5, Name = "Product5", Price = 30, CategoryId = 2, Category = cat2},
                    new Product {Id = 6, Name = "Product6", Price = 10, CategoryId = 2, Category = cat2},
                    new Product {Id = 8, Name = "Product8", Price = 10, CategoryId = 2, Category = cat2}
                });

            mockProd.Setup(m => m.GetProductsCount(null)).Returns(15);
            mockProd.Setup(m => m.GetProductsCount(1)).Returns(6);
            mockProd.Setup(m => m.GetProductsCount(2)).Returns(9);

            ProductController controller = new ProductController(mockProd.Object, mockCat.Object);

            Product[] result = ((ProductsListViewModel)controller.List(null, pageNumber).Model).Products.ToArray();

            Assert.AreEqual(result.Length, 5);

            Product[] result1 = ((ProductsListViewModel)controller.List(1, pageNumber).Model).Products.ToArray();

            Assert.AreEqual(result1.Length, 2);
            Assert.IsTrue(result1[0].Category.Name == "Cat1");

            Product[] result2 = ((ProductsListViewModel)controller.List(2, pageNumber).Model).Products.ToArray();

            Assert.AreEqual(result2.Length, 5);
            Assert.IsTrue(result2[0].Category.Name == "Cat2");
        }

        [TestMethod]
        public void ProductList_ManyProductsPages_CountPerCategory()
        {
            int pageSize = 5;
            int pageNumber = 1;
            Mock<IProductRepository> mockProd = new Mock<IProductRepository>();
            Mock<ICategoryRepository> mockCat = new Mock<ICategoryRepository>();

            Category cat1 = new Category
            {
                Id = 1,
                Name = "Cat1"
            };
            Category cat2 = new Category
            {
                Id = 2,
                Name = "Cat2"
            };

            mockProd.Setup(m => m.GetProductsByCategory(pageSize, pageNumber, null))
                .Returns(new Product[]
                {
                    new Product {Id = 5, Name = "Product5", Price = 30, CategoryId = 2, Category = cat2},
                    new Product {Id = 6, Name = "Product6", Price = 10, CategoryId = 1, Category = cat1},
                    new Product {Id = 7, Name = "Product7", Price = 20, CategoryId = 1, Category = cat1}
                });
            mockProd.Setup(m => m.GetProductsByCategory(pageSize, pageNumber, 1))
                .Returns(new Product[]
                {
                    new Product {Id = 5, Name = "Product5", Price = 30, CategoryId = 2, Category = cat2}
                });
            mockProd.Setup(m => m.GetProductsByCategory(pageSize, pageNumber, 2))
                .Returns(new Product[]
                {
                    new Product {Id = 6, Name = "Product6", Price = 10, CategoryId = 1, Category = cat1},
                    new Product {Id = 7, Name = "Product7", Price = 20, CategoryId = 1, Category = cat1}
                });

            mockProd.Setup(m => m.GetProductsCount(null)).Returns(10);
            mockProd.Setup(m => m.GetProductsCount(1)).Returns(3);
            mockProd.Setup(m => m.GetProductsCount(2)).Returns(7);

            ProductController controller = new ProductController(mockProd.Object, mockCat.Object);

            int result1 = ((ProductsListViewModel) controller.List(null).Model).PagingInfo.TotalItems;
            int result2 = ((ProductsListViewModel)controller.List(1, pageNumber).Model).PagingInfo.TotalItems;
            int result3 = ((ProductsListViewModel)controller.List(2, pageNumber).Model).PagingInfo.TotalItems;

            Assert.AreEqual(result1, 10);
            Assert.AreEqual(result2, 3);
            Assert.AreEqual(result3, 7);
        }

        [TestMethod]
        public void ProductList_ManyProductsPages_PageLinksGenerated()
        {
            HtmlHelper helper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                ItemsPerPage = 10,
                TotalItems = 25
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            MvcHtmlString result = helper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
            + @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }


        [TestMethod]
        public void GetImage_ValidProductId_RetriveImageData()
        {
            Product product = new Product
            {
                Id = 1,
                Name = "test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.GetProductById(1))
                .Returns(product);

            ProductController controller = new ProductController(mock.Object, null);

            ActionResult result = controller.GetImage(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(product.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void GetImage_InvalidProductId_ReturnsNull()
        {
            Product product = new Product
            {
                Id = 1,
                Name = "test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.GetProductById(1))
                .Returns(product);

            ProductController controller = new ProductController(mock.Object, null);

            ActionResult result = controller.GetImage(10);

            Assert.IsNull(result);
        }
    }
}
