using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CuStore.WebUI.Infrastructure.Abstract;
using CuStore.WebUI.Infrastructure.Helpers;
using Newtonsoft.Json.Linq;

namespace CuStore.WebUI.Infrastructure.Implementations
{
    public class GoogleMapsApiClient : IPlacesApiClient
    {
        private readonly string _apiKey = "AIzaSyAr_kuboi8vvnQjkPkh4_NLLGF1f4XbQdc";
        private readonly ICountriesProvider _countriesProvider;

        public static Func<HttpClient> ClientFactory = () => new HttpClient();

        public GoogleMapsApiClient(ICountriesProvider countriesProvider)
        {
            _countriesProvider = countriesProvider;
        }

        public async Task<List<string>> GetCities(string cityNamePrefix, string country)
        {
            using (var client = ClientFactory())
            {
                List<string> cities = new List<string>();

                var url = new UriBuilder("https://maps.googleapis.com/maps/api/place/autocomplete/" +
                                         "json?&key=" +
                                         _apiKey +
                                         "&types=(cities)&input=" +
                                         cityNamePrefix);

                //HTTP GET
                var response = await client.GetAsync(url.ToString());
                var result = await response.Content.ReadAsStringAsync();

                JObject obj = JObject.Parse(result);

                foreach (var item in obj.GetValue("predictions"))
                {
                    var cityWithDescriptionJson = item.SelectToken("description").ToString();

                    var city = ParseResponseItem(cityWithDescriptionJson, country);

                    if (city != null)
                    {
                        cities.Add(city);
                    }
                }

                return cities;
            }
        }

        private string ParseResponseItem(string cityWithDescriptionJson, string country = null)
        {
            string city = null;

            if (country.IsNullOrEmpty())
            {
                city = cityWithDescriptionJson;
            }

            var strings = cityWithDescriptionJson.Split(',');

            if (strings.Length > 0)
            {
                string countryJson = strings[strings.Length - 1]?.TrimStart(' ');

                if (countryJson != null && _countriesProvider.AreCountryNameEqual(countryJson, country))
                {
                    city = cityWithDescriptionJson;
                }
            }

            return city;
        }
    }
}