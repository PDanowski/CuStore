using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuStore.WebUI.Infrastructure.Abstract
{
    public interface ICrmClient
    {
        Guid CreateCustomerData(string customerCode, int bonusPoints);
        bool AddPointsForCustomer(Guid customerGuid, int points);
        bool RemoveCustomerData(Guid customerGuid);
        int GetPointsForCustomer(Guid customerGuid);
    }
}