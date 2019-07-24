using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.WebUI.Controllers;
using CuStore.WebUI.Infrastructure.Helpers;
using CuStore.WebUI.Infrastructure.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CuStore.UnitTests.Controllers
{
    [TestClass]
    public class UserAddressControllerTests
    {
        [TestMethod]
        public void EditUserAddress_ForExistingAddress_ReturnsAddres()
        {
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);

            UserAddress userAddress = new UserAddress
            {
                Id = 1,
                UserId = new Guid().ToString()
            };
            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            mock.Setup(m => m.GetUserAddress(It.IsAny<string>())).Returns(userAddress);

            //Set your controller ControllerContext with fake context
            UserAddressController controller = new UserAddressController(mock.Object, null, new CountriesProvider())
                { ControllerContext = controllerContext.Object };

            UserAddress result = (UserAddress)controller.Edit().ViewData.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, userAddress.Id);
            Assert.AreEqual(result.UserId, userAddress.UserId);
        }

        [TestMethod]
        public void EditUserAddress_ValidData_ChangesSaved()
        {
            UserAddress userAddress = new UserAddress
            {
                Id = 1,
                UserId = new Guid().ToString()
            };

            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            fakeIdentity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "UserId"));
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);

            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            mock.Setup(m => m.AddOrSaveUserAddress(It.IsAny<UserAddress>())).Returns(true);

            //Set your controller ControllerContext with fake context
            UserAddressController controller = new UserAddressController(mock.Object, null, new CountriesProvider())
            {
                ControllerContext = controllerContext.Object
            };

            var result = controller.Edit(userAddress);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void EditUserAddress_InvalidData_ChangesRejected()
        {
            UserAddress userAddress = new UserAddress
            {
                Id = 1,
                UserId = new Guid().ToString()
            };

            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            fakeIdentity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "UserId"));
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);

            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            mock.Setup(m => m.GetUserAddress(It.IsAny<string>())).Returns((UserAddress)null);

            //Set your controller ControllerContext with fake context
            UserAddressController controller = new UserAddressController(mock.Object, null, new CountriesProvider())
            {
                ControllerContext = controllerContext.Object
            };
            controller.ModelState.AddModelError("error", "error");

            var result = controller.Edit(userAddress);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void EditUserAddress_ForNotExistingAddress_ReturnsNewAddress()
        {
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            fakeIdentity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "UserId"));
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);

            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            mock.Setup(m => m.AddOrSaveUserAddress(It.IsAny<UserAddress>())).Returns(true);

            //Set your controller ControllerContext with fake context
            UserAddressController controller = new UserAddressController(mock.Object, null, new CountriesProvider())
                { ControllerContext = controllerContext.Object };

            UserAddress result = (UserAddress)controller.Edit().ViewData.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 0);
            Assert.AreEqual(result.UserId, "UserId");
        }
    }
}
