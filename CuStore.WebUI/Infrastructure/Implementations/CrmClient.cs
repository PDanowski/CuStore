using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CuStore.WebUI.CrmService;
using CuStore.WebUI.Infrastructure.Abstract;

namespace CuStore.WebUI.Infrastructure.Implementations
{
    public class CrmClient : ICrmClient
    {

        public Guid CreateCustomerData(string customerCode, int bonusPoints)
        {
            using (var client = new CrmServiceClient())
            {
                return client.AddCustomer(customerCode, bonusPoints);
            }
        }

        public bool AddPointsForCustomer(Guid customerGuid, int points)
        {
            using (var client = new CrmServiceClient())
            {
                return client.AddPointForCustomer(customerGuid, points);
            }
        }

        public bool RemoveCustomerData(Guid customerGuid)
        {
            using (var client = new CrmServiceClient())
            {
                return client.RemoveCustomer(customerGuid);
            }
        }

        public int GetPointsForCustomer(Guid customerGuid)
        {
            using (var client = new CrmServiceClient())
            {
                return client.GetPoints(customerGuid);
            }
        }
    }
}