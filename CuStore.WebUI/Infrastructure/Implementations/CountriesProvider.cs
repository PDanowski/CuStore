using System;
using System.Collections.Generic;
using System.Globalization;
using CuStore.WebUI.Infrastructure.Abstract;

namespace CuStore.WebUI.Infrastructure.Implementations
{
    public class CountriesProvider : ICountriesProvider
    {
        private readonly CultureInfo[] _cInfoList;

        public CountriesProvider()
        {
            _cInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        }

        public List<string> FillCountryList()
        {
            List<string> countryList = new List<string>();

            foreach (CultureInfo cInfo in _cInfoList)
            {
                RegionInfo regionInfo = new RegionInfo(cInfo.LCID);
                if (!(countryList.Contains(regionInfo.EnglishName)))
                {
                    countryList.Add(regionInfo.EnglishName);
                }
            }

            countryList.Sort();

            return countryList;
        }

        private RegionInfo GetCountryByEnglishName(string countryEnglishName)
        {
            foreach (CultureInfo cInfo in _cInfoList)
            {
                RegionInfo regionInfo = new RegionInfo(cInfo.LCID);
                if (regionInfo.EnglishName.Equals(countryEnglishName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return regionInfo;
                }
            }

            return null;
        }

        public bool AreCountryNameEqual(string countryName, string countryEnglishName)
        {
            if (countryName == null)
            {
                return false;
            }

            if (countryName.Equals(countryEnglishName, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            RegionInfo region = GetCountryByEnglishName(countryEnglishName);

            if (region == null)
            {
                return false;
            }

            if (CheckIfRegionMatchCountryName(region, countryName))
            {
                return true;
            }

            if (CheckIfRegionShortcutMatchCountryName(region, countryName))
            {
                return true;
            }

            return false;
        }

        private bool CheckIfRegionMatchCountryName(RegionInfo region, string countryName)
        {
            if (countryName.Contains(region.Name)
                || countryName.Contains(region.EnglishName)
                || countryName.Contains(region.NativeName)
                || countryName.Contains(region.DisplayName))
            {
                return true;
            }

            return false;
        }

        private bool CheckIfRegionShortcutMatchCountryName(RegionInfo region, string countryName)
        {
            if (region.EnglishName.Split().Length <= 1 
                || region.NativeName.Split().Length <= 1 
                || region.DisplayName?.Split().Length <= 1)
            {
                return false;
            }

            if (countryName.Contains(GetCountryShortcut(region.EnglishName))
                 || countryName.Contains(GetCountryShortcut(region.NativeName))
                 || countryName.Contains(GetCountryShortcut(region.DisplayName)))
            {
                return true;
            }

            return false;
        }

        private string GetCountryShortcut(string countryName)
        {
            string ret = "";
            string[] strSplit = countryName.Split();

            foreach (string res in strSplit)
            {
                ret += res.Substring(0, 1);
            }

            return ret;
        }
    }
}