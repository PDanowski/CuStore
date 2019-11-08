using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.CRMService.OperationContracts;

namespace CuStore.CRMService.DAL.Abstract
{
    public interface ICustomerDataProvider
    {
        CustomerData GetCustomerData(Guid customerId);
        int GetPoints(Guid customerId);
        Guid AddCustomer(string externalCode, int bonusPoints = 0);
        bool RemoveCustomer(Guid customerId);
        bool AddPointForCustomer(Guid customerId, int points);
        bool AddCustomer(CustomerData customerData);
    }
}
