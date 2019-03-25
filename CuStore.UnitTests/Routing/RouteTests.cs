using System;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using CuStore.WebUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CuStore.UnitTests.Routing
{
    [TestClass]
    public class RouteTests
    {
        private HttpContextBase CreateHttpContext(string httpMethod, string targetUrl = null)
        {
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath).Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);

            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);

            return mockContext.Object;
        }

        private void TestRouteMatch(string url, string controller, string action, string httpMethod,
            object routeProperties = null)
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            RouteData result = routes.GetRouteData(CreateHttpContext(httpMethod, url));

            Assert.IsNotNull(result);
            Assert.IsTrue(TestIncomingRouteResult(result, controller, action, routeProperties));
        }

        private bool TestIncomingRouteResult(RouteData routeResult, string controller, string action,
            object propertySet = null)
        {

            Func<object, object, bool> valCompare = (v1, v2) =>
            {
                return StringComparer.InvariantCultureIgnoreCase.Compare(v1, v2) == 0;
            };

            bool result = valCompare = (routeResult.Values["controller"], controller) &&
                          valCompare = (routeResult.Values["action"], action);

            if (propertySet != null)
            {
                PropertyInfo[] propInfo = propertySet.GetType().GetProperties();

                foreach (var prop in propInfo)
                {
                    if (!(routeResult.Values.ContainsKey(prop.Name)) 
                        && valCompare = (routeResult.Values[prop.Name], prop.GetValue(propertySet, null)))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private void TestRouteFail(string url, string httpMehtod)
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            RouteData result = routes.GetRouteData(CreateHttpContext(httpMehtod, url));

            Assert.IsTrue(result == null || result.Route == null);
        }

        [TestMethod]
        public void TestIncommingRoutes()
        {
            TestRouteMatch("~/Admin/Index", "Admin", "Index", "GET");
            TestRouteMatch("~/One/Two", "One", "Two", "GET");
            TestRouteMatch("~/Product/List", "Product", "List", "GET", new { categoryId = "1"});
            TestRouteMatch("~/Page_test", "Product", "List", "GET");
            TestRouteMatch("~/Page_1", "Product", "List", "POST");

            TestRouteFail("~/Admin/Index/Index/Index", "GET");
            TestRouteFail("~/Admin/Index/Index/Index/Index", "GET");

        }
    }

}
