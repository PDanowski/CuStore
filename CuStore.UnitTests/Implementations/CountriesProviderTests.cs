using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.WebUI.Infrastructure.Abstract;
using CuStore.WebUI.Infrastructure.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CuStore.UnitTests.Implementations
{
    [TestClass]
    public class CountriesProviderTests
    {
        [TestMethod]
        public void FillCountryList_UseAllCultures_ReturnsCountries()
        {
            ICountriesProvider provider = new CountriesProvider();
            CultureInfo[] cInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            var count = cInfoList.Select(x => new RegionInfo(x.LCID).EnglishName).Distinct().Count();

            List<string> result = provider.FillCountryList();

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, count);
        }

        [TestMethod]
        public void AreCountryNameEqual_BothCountryNamesProvided_ReturnsMatch()
        {
            ICountriesProvider provider = new CountriesProvider();

            bool result = provider.AreCountryNameEqual("UK", "United Kingdom");
            bool result2 = provider.AreCountryNameEqual("GB", "United Kingdom");
            bool result3 = provider.AreCountryNameEqual("United Kingdom", "United Kingdom");

            Assert.AreEqual(result, true);
            Assert.AreEqual(result2, true);
            Assert.AreEqual(result3, true);
        }

        [TestMethod]
        public void AreCountryNameEqual_OnlyCountryNameProvided_ReturnsNotMatch()
        {
            ICountriesProvider provider = new CountriesProvider();

            bool result = provider.AreCountryNameEqual("UK", null);
            bool result2 = provider.AreCountryNameEqual("GB", null);
            bool result3 = provider.AreCountryNameEqual("United Kingdom", null);

            Assert.AreEqual(result, false);
            Assert.AreEqual(result2, false);
            Assert.AreEqual(result3, false);
        }

        [TestMethod]
        public void AreCountryNameEqual_OnlyCountryEnglishNameProvided_ReturnsNotMatch()
        {
            ICountriesProvider provider = new CountriesProvider();

            bool result = provider.AreCountryNameEqual(null, "United Kingdom");
            bool result2 = provider.AreCountryNameEqual(null, "United Kingdom");
            bool result3 = provider.AreCountryNameEqual(null, "United Kingdom");

            Assert.AreEqual(result, false);
            Assert.AreEqual(result2, false);
            Assert.AreEqual(result3, false);
        }
    }
}
