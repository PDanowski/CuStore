using System.Linq;
using System.Net.Http;
using CuStore.UnitTests.Helpers;
using CuStore.WebUI.Infrastructure.Abstract;
using CuStore.WebUI.Infrastructure.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CuStore.UnitTests.Implementations
{
    [TestClass]
    public class GoogleMapsApiClientTests
    {
        private readonly HttpMessageHelper _httpMessageHelper;

        public GoogleMapsApiClientTests()
        {
            _httpMessageHelper = new HttpMessageHelper();
        }

        [TestMethod]
        public void GetCities_CountryAndPrefixProvided_ReturnsCitiesCollectionFiltredByCountry()
        {
            // use real http client with mocked handler here
            var httpClient = new HttpClient(_httpMessageHelper.CreateMock(JsonResponseContent.GoogleMapsApiCity1).Object);

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
            // use real http client with mocked handler here
            var httpClient = new HttpClient(_httpMessageHelper.CreateMock(JsonResponseContent.GoogleMapsApiCity1).Object);

            IPlacesApiClient client = new GoogleMapsApiClient(new CountriesProvider());
            GoogleMapsApiClient.ClientFactory = () => httpClient;


            var result = client.GetCities("Warszawa", null).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 2);
            Assert.IsTrue(result.First().Contains("Warszawa"));
        }


        [TestMethod]
        public void GetCities_NoValuesProvided_ReturnsEmptyCollection()
        {
            // use real http client with mocked handler here
            var httpClient = new HttpClient(_httpMessageHelper.CreateMock(JsonResponseContent.InvalidRequest).Object);

            IPlacesApiClient client = new GoogleMapsApiClient(new CountriesProvider());
            GoogleMapsApiClient.ClientFactory = () => httpClient;


            var result = client.GetCities("", null).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 0);
        }

        [TestMethod]
        public void GetCities_WrongCountryAndPrefixProvided_ReturnsCitiesCollectionFiltredByGivenCountry()
        {
            // use real http client with mocked handler here
            var httpClient = new HttpClient(_httpMessageHelper.CreateMock(JsonResponseContent.GoogleMapsApiCity1).Object);

            IPlacesApiClient client = new GoogleMapsApiClient(new CountriesProvider());
            GoogleMapsApiClient.ClientFactory = () => httpClient;


            var result = client.GetCities("Warszawa", "Spain").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 0);
        }

        [TestMethod]
        public void GetCities_NullValuesProvided_ReturnsEmptyCollection()
        {
            // use real http client with mocked handler here
            var httpClient = new HttpClient(_httpMessageHelper.CreateMock(JsonResponseContent.InvalidRequest).Object);

            IPlacesApiClient client = new GoogleMapsApiClient(new CountriesProvider());
            GoogleMapsApiClient.ClientFactory = () => httpClient;


            var result = client.GetCities(null, null).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 0);
        }
    }
}
