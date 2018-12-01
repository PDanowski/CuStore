using System;
using System.Linq;
using System.Web.Mvc;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.WebUI.Controllers;
using CuStore.WebUI.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CuStore.UnitTests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.GetProductById(It.IsAny<int>()))
                .Returns(new Product { Id = 1, Name = "Product1", Price = 20, CategoryId = 1 });

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object);

            controller.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.CartItems.Count, 1);
            Assert.AreEqual(cart.CartItems.ToArray()[0].Product.Id, 1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_Index()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.GetProductById(It.IsAny<int>()))
                .Returns(new Product { Id = 1, Name = "Product1", Price = 20, CategoryId = 1 });

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object);

            RedirectToRouteResult result = controller.RemoveFromCart(cart, 1, "returnUrl");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "returnUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Index()
        {
            Cart cart = new Cart();

            CartController controller = new CartController(null);

            CartIndexViewModel result = (CartIndexViewModel)controller.Index(cart, "returnUrl").Model;

            Assert.AreEqual(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "returnUrl");
        }

    }
}
