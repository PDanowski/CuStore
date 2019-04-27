using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
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
            Mock<IProductRepository> mockProd = new Mock<IProductRepository>();
            Mock<ICartRepository> mockCart = new Mock<ICartRepository>();
            mockProd.Setup(m => m.GetProductById(It.IsAny<int>()))
                .Returns(new Product { Id = 1, Name = "Product1", Price = 20, CategoryId = 1 });

            Cart cart = new Cart();

            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);

            //Set your controller ControllerContext with fake context
            CartController controller =
                new CartController(mockProd.Object, mockCart.Object, null, null, null, null)
                    { ControllerContext = controllerContext.Object };

            controller.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.CartItems.Count, 1);
            Assert.AreEqual(cart.CartItems.ToArray()[0].Product.Id, 1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_Index()
        {
            Mock<IProductRepository> mockProd = new Mock<IProductRepository>();
            Mock<ICartRepository> mockCart = new Mock<ICartRepository>();
            mockProd.Setup(m => m.GetProductById(It.IsAny<int>()))
                .Returns(new Product { Id = 1, Name = "Product1", Price = 20, CategoryId = 1 });

            Cart cart = new Cart();

            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);

            //Set your controller ControllerContext with fake context
            CartController controller =
                new CartController(mockProd.Object, mockCart.Object, null, null, null, null)
                    { ControllerContext = controllerContext.Object};   

            RedirectToRouteResult result = controller.RemoveFromCart(cart, 1, "returnUrl");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "returnUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Index()
        {
            Cart cart = new Cart();

            CartController controller = new CartController(null, null, null, null, null, null);

            CartIndexViewModel result = (CartIndexViewModel)controller.Index(cart, "returnUrl").Model;

            Assert.AreEqual(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "returnUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IEmailSender> mock = new Mock<IEmailSender>();
            Mock<IShippingMethodRepository> mockRepo = new Mock<IShippingMethodRepository>();
            mockRepo.Setup(m => m.GetShippingMethods())
                .Returns(new List<ShippingMethod>{});

            Cart cart = new Cart();

            CheckoutViewModel viewModel = new CheckoutViewModel
            {
                Cart = cart,
                OrderValue = 0.00M,
                SelectedShippingMethodId = 0,
                ShippingMethods = null
            };

            CartController controller = new CartController(null, null, mockRepo.Object, null, null, mock.Object);

            ViewResult result = controller.Checkout(viewModel, cart);

            mock.Verify(m => m.ProcessOrder(It.IsAny<Order>()), Times.Never);

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_Address()
        {
            Mock<IEmailSender> mock = new Mock<IEmailSender>();
            Mock<IShippingMethodRepository> mockRepo = new Mock<IShippingMethodRepository>();
            mockRepo.Setup(m => m.GetShippingMethods())
                .Returns(new List<ShippingMethod> { });

            Cart cart = new Cart();
            cart.AddProduct(new Product
            {
                CategoryId = 1,
                Id = 1,
                Name = "test",
                Price = 1.89M
            }, 1);

            CheckoutViewModel viewModel = new CheckoutViewModel
            {
                Cart = cart,
                OrderValue = 0.00M,
                SelectedShippingMethodId = 0,
                ShippingMethods = null
            };

            CartController controller = new CartController(null, null, mockRepo.Object, null, null, mock.Object);
            controller.ModelState.AddModelError("error", @"error");

            ViewResult result = controller.Checkout(viewModel, cart);

            mock.Verify(m => m.ProcessOrder(It.IsAny<Order>()), Times.Never);

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            Mock<IEmailSender> mock = new Mock<IEmailSender>();
            Mock<IOrderRepository> mockOrder = new Mock<IOrderRepository>();
            Mock<IShippingMethodRepository> mockShip = new Mock<IShippingMethodRepository>();

            mockOrder.Setup(m => m.AddOrder(It.IsAny<Order>())).Returns(true);

            Cart cart = new Cart();
            cart.AddProduct(new Product
            {
                CategoryId = 1,
                Id = 1,
                Name = "test",
                Price = 1.89M
            }, 1);

            CheckoutViewModel viewModel = new CheckoutViewModel
            {
                Cart = cart,
                OrderValue = 0.00M,
                SelectedShippingMethodId = 0,
                ShippingMethods = null
            };

            CartController controller = new CartController(null, null, mockShip.Object, mockOrder.Object, null, mock.Object);

            ViewResult result = controller.Checkout(viewModel, cart);

            mock.Verify(m => m.ProcessOrder(It.IsAny<Order>()), Times.Once);

            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
