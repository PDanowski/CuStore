using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuStore.WebUI.Infrastructure.Abstract
{
    public interface ICountriesProvider
    {
        List<string> FillCountryList();
        bool AreCountryNameEqual(string countryName, string countryEnglishName);
    }
}