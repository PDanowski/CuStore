using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CuStore.WebUI.Infrastructure.Abstract
{
    public interface IPlacesApiClient
    {
        Task<List<string>> GetCities(string cityNamePrefix, string country);
    }
}
