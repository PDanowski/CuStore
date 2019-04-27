using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.Domain.Entities.Enums;
using CuStore.WebUI.Areas.Admin.Controllers;
using CuStore.WebUI.Areas.Admin.ViewModels;
using CuStore.WebUI.Controllers;
using CuStore.WebUI.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CuStore.UnitTests.Controllers
{
    [TestClass]
    public class AdminControllerTests
    {
        [TestMethod]
        public void Get_Products_Contains_All_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.GetProductsByCategory(It.IsAny<int>(), It.IsAny<int>(), null)).Returns(new Product[]
            {
                new Product {Id = 1, Name = "Product1", Price = 10, CategoryId = 1},
                new Product {Id = 2, Name = "Product2", Price = 20, CategoryId = 2},
                new Product {Id = 3, Name = "Product3", Price = 30, CategoryId = 3},
                new Product {Id = 4, Name = "Product4", Price = 10, CategoryId = 1},
                new Product {Id = 5, Name = "Product5", Price = 20, CategoryId = 2}
            });

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(mock.Object, null, null);

            Product[] result = ((IEnumerable<Product>) controller.GetProducts().ViewData.Model).ToArray();

            Assert.AreEqual(result.Length, 5);
            Assert.AreEqual("Product1", result[0].Name);
            Assert.AreEqual("Product5", result[4].Name);
        }

        [TestMethod]
        public void Manage_Categories_Contains_All_Categories()
        {
            Mock<ICategoryRepository> mock = new Mock<ICategoryRepository>();
            mock.Setup(m => m.GetCategories()).Returns(new Category[]
            {
                new Category {Id = 1, Name = "Category1", ParentCategoryId = null},
                new Category {Id = 2, Name = "Category2", ParentCategoryId = 1},
                new Category {Id = 3, Name = "Category3", ParentCategoryId = 1},
                new Category {Id = 4, Name = "Category4", ParentCategoryId = 1},
                new Category {Id = 5, Name = "Category5", ParentCategoryId = 1}
            });

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(null, mock.Object, null);

            Category[] result = ((IEnumerable<Category>)controller.ManageCategories().ViewData.Model).ToArray();

            Assert.AreEqual(result.Length, 5);
            Assert.AreEqual("Category1", result[0].Name);
            Assert.AreEqual("Category5", result[4].Name);
        }

        [TestMethod]
        public void Get_Orders_Contains_All_Orders()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            mock.Setup(m => m.GetOrders(It.IsAny<int>(), It.IsAny<int>())).Returns(new Order[]
            {
                new Order {Id = 1, OrderDate = DateTime.Now, Cart = new Cart(), Status = OrderStatus.Accepted},
                new Order {Id = 2, OrderDate = DateTime.Now, Cart = new Cart(), Status = OrderStatus.Accepted},
                new Order {Id = 3, OrderDate = DateTime.Now, Cart = new Cart(), Status = OrderStatus.Accepted},
                new Order {Id = 4, OrderDate = DateTime.Now, Cart = new Cart(), Status = OrderStatus.Accepted},
                new Order {Id = 5, OrderDate = DateTime.Now, Cart = new Cart(), Status = OrderStatus.Accepted}
            });

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(null, null, mock.Object);

            Order[] result = ((IEnumerable<Order>)controller.GetOrders().ViewData.Model).ToArray();

            Assert.AreEqual(result.Length, 5);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(5, result[4].Id);
        }

        [TestMethod]
        public void Manage_Categories_Can_Edit()
        {
            Mock<ICategoryRepository> mock = new Mock<ICategoryRepository>();
            mock.Setup(m => m.GetCategoryById(1))
                .Returns(new Category { Id = 1, Name = "Category1", ParentCategoryId = 1});
            mock.Setup(m => m.GetCategoryById(2))
                .Returns(new Category { Id = 2, Name = "Category2", ParentCategoryId = 1 });
            mock.Setup(m => m.GetCategoryById(3))
                .Returns(new Category { Id = 3, Name = "Category3", ParentCategoryId = 1 });

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(null, mock.Object, null);

            Category c1 = (controller.EditCategory(1).ViewData.Model as EditCategoryViewModel)?.Category;
            Category c2 = (controller.EditCategory(2).ViewData.Model as EditCategoryViewModel)?.Category;
            Category c3 = (controller.EditCategory(3).ViewData.Model as EditCategoryViewModel)?.Category;

            Assert.AreEqual(1, c1.Id);
            Assert.AreEqual(2, c2.Id);
            Assert.AreEqual(3, c3.Id);
        }

        [TestMethod]
        public void Manage_Products_Can_Edit()
        {
            Mock<IProductRepository> mockProd = new Mock<IProductRepository>();
            Mock<ICategoryRepository> mockCat = new Mock<ICategoryRepository>();

            mockProd.Setup(m => m.GetProductById(1))
                .Returns(new Product { Id = 1, Name = "Product1", Price = 10, CategoryId = 1 });
            mockProd.Setup(m => m.GetProductById(2))
                .Returns(new Product { Id = 2, Name = "Product2", Price = 20, CategoryId = 2 });
            mockProd.Setup(m => m.GetProductById(3))
                .Returns(new Product { Id = 3, Name = "Product3", Price = 30, CategoryId = 3 });

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(mockProd.Object, mockCat.Object, null);

            Product p1 = (controller.EditProduct(1).ViewData.Model as EditProductViewModel)?.Product;
            Product p2 = (controller.EditProduct(2).ViewData.Model as EditProductViewModel)?.Product;
            Product p3 = (controller.EditProduct(3).ViewData.Model as EditProductViewModel)?.Product;

            Assert.AreEqual(1, p1.Id);
            Assert.AreEqual(2, p2.Id);
            Assert.AreEqual(3, p3.Id);
        }

        [TestMethod]
        public void Manage_Orders_Can_Edit()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            mock.Setup(m => m.GetOrderById(1))
                .Returns(new Order { Id = 1, OrderDate = DateTime.Now, Cart = new Cart(), Status = OrderStatus.Accepted });
            mock.Setup(m => m.GetOrderById(2))
                .Returns(new Order { Id = 2, OrderDate = DateTime.Now, Cart = new Cart(), Status = OrderStatus.Archived });
            mock.Setup(m => m.GetOrderById(3))
                .Returns(new Order { Id = 3, OrderDate = DateTime.Now, Cart = new Cart(), Status = OrderStatus.Realized });

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(null, null, mock.Object);

            Order o1 = (controller.EditOrder(1).ViewData.Model as Order);
            Order o2 = (controller.EditOrder(2).ViewData.Model as Order);
            Order o3 = (controller.EditOrder(3).ViewData.Model as Order);

            Assert.AreEqual(1, o1?.Id);
            Assert.AreEqual(2, o2?.Id);
            Assert.AreEqual(3, o3?.Id);
        }

        [TestMethod]
        public void Manage_Categories_Edit_Not_Existing()
        {
            Mock<ICategoryRepository> mock = new Mock<ICategoryRepository>();
            mock.Setup(m => m.GetCategories()).Returns(new Category[]
            {
                new Category {Id = 1, Name = "Product1", ParentCategoryId = null},
                new Category {Id = 2, Name = "Product2", ParentCategoryId = 1},
                new Category {Id = 3, Name = "Product3", ParentCategoryId = 1}
            });

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(null, mock.Object, null);

            Category c1 = controller.EditCategory(8).ViewData.Model as Category;

            Assert.IsNull(c1);
        }

        [TestMethod]
        public void Manage_Products_Edit_Not_Existing()
        {
            Mock<IProductRepository> mockProd = new Mock<IProductRepository>();
            Mock<ICategoryRepository> mockCat = new Mock<ICategoryRepository>();

            mockProd.Setup(m => m.GetProducts(true)).Returns(new Product[]
            {
                new Product {Id = 1, Name = "Product1", Price = 10, CategoryId = 1},
                new Product {Id = 2, Name = "Product2", Price = 20, CategoryId = 2},
                new Product {Id = 3, Name = "Product3", Price = 30, CategoryId = 3}
            });

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(mockProd.Object, mockCat.Object, null);

            Product p1 = controller.EditProduct(8).ViewData.Model as Product;

            Assert.IsNull(p1);
        }

        [TestMethod]
        public void Manage_Orders_Edit_Not_Existing()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            mock.Setup(m => m.GetOrderById(1)).Returns( 
                new Order {Id = 1, OrderDate = DateTime.Now, Cart = new Cart(), Status = OrderStatus.Accepted}
            );

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(null, null, mock.Object);

            Order o1 = controller.EditOrder(8).ViewData.Model as Order;

            Assert.IsNull(o1);
        }

        [TestMethod]
        public void Manage_Products_Save_Valid_Changes()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.SaveProduct(It.IsAny<Product>())).Returns(true);

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(mock.Object, null, null);

            Product p1 = new Product
            {
                Name = "test"
            };
            EditProductViewModel viewModel = new EditProductViewModel
            {
                Product = p1
            };

            ActionResult result = controller.EditProduct(viewModel);

            mock.Verify(m => m.SaveProduct(viewModel.Product));

            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Manage_Categories_Save_Valid_Changes()
        {
            Mock<ICategoryRepository> mock = new Mock<ICategoryRepository>();
            mock.Setup(m => m.SaveCategory(It.IsAny<Category>())).Returns(true);

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(null, mock.Object, null);

            Category c1 = new Category
            {
                Name = "test"
            };
            EditCategoryViewModel viewModel = new EditCategoryViewModel
            {
                Category = c1
            };

            ActionResult result = controller.EditCategory(viewModel);

            mock.Verify(m => m.SaveCategory(viewModel.Category));

            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Manage_Orders_Save_Valid_Changes()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            mock.Setup(m => m.SaveOrder(It.IsAny<Order>())).Returns(true);

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(null, null, mock.Object);

            Order o1 = new Order
            {
                Id = 1,
                OrderDate = DateTime.Today,
                Status = OrderStatus.Accepted
            };

            ActionResult result = controller.EditOrder(o1);

            mock.Verify(m => m.SaveOrder(o1));

            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Manage_Products_Save_Invalid_Changes()
        {
            Mock<IProductRepository> mockProd = new Mock<IProductRepository>();
            Mock<ICategoryRepository> mockCat = new Mock<ICategoryRepository>();

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(mockProd.Object, mockCat.Object, null);

            Product p1 = new Product
            {
                Name = "test"
            };
            EditProductViewModel viewModel = new EditProductViewModel
            {
                Product = p1
            };

            controller.ModelState.AddModelError("error", "error");

            ActionResult result = controller.EditProduct(viewModel);

            mockProd.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Manage_Categories_Save_Invalid_Changes()
        {
            Mock<ICategoryRepository> mock = new Mock<ICategoryRepository>();

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(null, mock.Object, null);

            Category c1 = new Category
            {
                Name = "test"
            };
            EditCategoryViewModel viewModel = new EditCategoryViewModel
            {
                Category = c1
            };

            controller.ModelState.AddModelError("error", "error");

            ActionResult result = controller.EditCategory(viewModel);

            mock.Verify(m => m.SaveCategory(It.IsAny<Category>()), Times.Never);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Manage_Orders_Save_Invalid_Changes()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(null, null, mock.Object);

            Order o1 = new Order
            {
                Id = 1,
                OrderDate = DateTime.Today,
                Status = OrderStatus.Accepted
            };

            controller.ModelState.AddModelError("error", "error");

            ActionResult result = controller.EditOrder(o1);

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Manage_Products_Delete_Valid()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.RemoveProduct(It.IsAny<int>())).Returns(true);

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(mock.Object, null, null);

            controller.ModelState.AddModelError("error", "error");

            controller.DeleteProduct(productId: 1);

            mock.Verify(m => m.RemoveProduct(1));
        }

        [TestMethod]
        public void Manage_Categories_Delete_Valid()
        {
            Mock<ICategoryRepository> mock = new Mock<ICategoryRepository>();
            mock.Setup(m => m.RemoveCategory(It.IsAny<int>())).Returns(true);

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(null, mock.Object, null);

            controller.ModelState.AddModelError("error", "error");

            controller.DeleteCategory(categoryId: 1);

            mock.Verify(m => m.RemoveCategory(1));
        }

        [TestMethod]
        public void Manage_Orders_Delete_Valid()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            mock.Setup(m => m.RemoveOrder(It.IsAny<int>())).Returns(true);

            WebUI.Areas.Admin.Controllers.ManageController controller = new WebUI.Areas.Admin.Controllers.ManageController(null, null, mock.Object);

            controller.ModelState.AddModelError("error", "error");

            controller.DeleteOrder(orderId: 1);

            mock.Verify(m => m.RemoveOrder(1));
        }

    }
}
