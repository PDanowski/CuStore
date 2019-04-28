using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CuStore.UnitTests.Controllers
{
    [TestClass]
    public class OrderControllerTests
    {
        [TestMethod]
        public void GetOrderById_ReturnsOrder()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            mock.Setup(m => m.GetOrderById(It.IsAny<int>())).Returns(new Order
            {
                Id = 1,
                Cart = new Cart
                {
                    CartItems = new List<CartItem>(),
                    Id = 1,
                    User = new User(),
                    UserId = Guid.NewGuid().ToString()
                },
                CartId = 1,
                OrderDate = DateTime.Today,
                ShippingAddress = new ShippingAddress(),
                ShippingAddressId = 2,
                ShippingMethod = new ShippingMethod(),
                ShippingMethodId = 3
            });

            OrderController controller = new OrderController(mock.Object);

            Order result = ((Order)controller.OrderDetails(1).Model);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 1);
            Assert.AreEqual(result.OrderDate, DateTime.Today);
        }

        [TestMethod]
        public void GetOrderById_ReturnsNull()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            mock.Setup(m => m.GetOrderById(1)).Returns(new Order
            {
                Id = 1,
                Cart = new Cart
                {
                    CartItems = new List<CartItem>(),
                    Id = 1,
                    User = new User(),
                    UserId = Guid.NewGuid().ToString()
                },
                CartId = 1,
                OrderDate = DateTime.Today,
                ShippingAddress = new ShippingAddress(),
                ShippingAddressId = 2,
                ShippingMethod = new ShippingMethod(),
                ShippingMethodId = 3
            });

            OrderController controller = new OrderController(mock.Object);

            Order result = ((Order)controller.OrderDetails(2).Model);

            Assert.IsNull(result);
        }
    }
}
