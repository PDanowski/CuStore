using System;
using System.Collections.Generic;
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

namespace CuStore.UnitTests
{
    [TestClass]
    public class ProductControllerTest
    {
        [TestMethod]
        public void ProductList_Can_Paginate()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.GetProductsByCategory(It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new Product[]
            {
                new Product {Id = 1, Name = "Product1", Price = 10, CategoryId = 1},
                new Product {Id = 2, Name = "Product2", Price = 20, CategoryId = 2},
                new Product {Id = 3, Name = "Product3", Price = 30, CategoryId = 3},
                new Product {Id = 4, Name = "Product4", Price = 10, CategoryId = 1},
                new Product {Id = 5, Name = "Product5", Price = 20, CategoryId = 2}
            });

            ProductController controller = new ProductController(mock.Object);
            ProductsListViewModel result = (ProductsListViewModel) controller.List(null, 1).Model;

            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 5);
            Assert.AreEqual(prodArray[0].Name, "Product1");
        }

        [TestMethod]
        public void ProductList_Can_Send_Paginatation_ViewModel()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.GetProductsByCategory(It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
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
            mock.Setup(m => m.GetProductsCount()).Returns(7);

            ProductController controller = new ProductController(mock.Object);
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.ItemsPerPage, 5);
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.TotalPages, 2);
            Assert.AreEqual(pageInfo.TotalItems, 7);
        }

        [TestMethod]
        public void ProductList_Can_Filter_Products()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

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

            mock.Setup(m => m.GetProductsByCategory(It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Product[]
                {
                    new Product {Id = 4, Name = "Product4", Price = 20, CategoryId = 2, Category = cat2},
                    new Product {Id = 5, Name = "Product5", Price = 30, CategoryId = 2, Category = cat2},
                    new Product {Id = 6, Name = "Product6", Price = 10, CategoryId = 2, Category = cat2},
                    new Product {Id = 7, Name = "Product7", Price = 20, CategoryId = 2, Category = cat2}
                });
            mock.Setup(m => m.GetProductsCount()).Returns(7);

            ProductController controller = new ProductController(mock.Object);
            Product[] result = ((ProductsListViewModel)controller.List(2, 1).Model).Products.ToArray();

            Assert.AreEqual(result.Length, 4);
            Assert.IsTrue(result[0].Category.Name == "Cat2");
        }

        [TestMethod]
        public void ProductList_Can_Generate_Page_Links()
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
    }
}
