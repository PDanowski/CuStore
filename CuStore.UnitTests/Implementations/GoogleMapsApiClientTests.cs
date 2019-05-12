using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CuStore.UnitTests.Helpers;
using CuStore.WebUI.Infrastructure.Abstract;
using CuStore.WebUI.Infrastructure.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;

namespace CuStore.UnitTests.Implementations
{
    [TestClass]
    public class GoogleMapsApiClientTests
    {

        [TestMethod]
        public void GetCities_CountryAndPrefixProvided_ReturnsCitiesCollectionFiltredByCountry()
        {
            // ARRANGE
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Default);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonResponseContent.GoogleMapsApiCity1)
                })
                .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object);

            IPlacesApiClient client = new GoogleMapsApiClient(new CountriesProvider());
            GoogleMapsApiClient.ClientFactory = () => httpClient;


            var result = client.GetCities("Warszawa", "Poland").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 1);
            Assert.IsTrue(result.First().Contains("Warszawa"));
        }


        [TestMethod]
        public void GetCities_OnlyPrefixProvided_ReturnsCompleteCitiesCollection()
        {
            // ARRANGE
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Default);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonResponseContent.GoogleMapsApiCity1)
                })
                .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object);

            IPlacesApiClient client = new GoogleMapsApiClient(new CountriesProvider());
            GoogleMapsApiClient.ClientFactory = () => httpClient;


            var result = client.GetCities("Warszawa", null).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 2);
            Assert.IsTrue(result.First().Contains("Warszawa"));
        }

    }
}
