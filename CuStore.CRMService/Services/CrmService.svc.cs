using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using CuStore.CRMService.DAL.Abstract;
using CuStore.CRMService.DataContracts;
using CuStore.CRMService.OperationContracts;

namespace CuStore.CRMService.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class CrmService : ICrmService
    {
        private readonly ICustomerDataProvider _provider;

        public CrmService(ICustomerDataProvider provider)
        {
            _provider = provider;
        }

        public CustomerData GetCustomerData(Guid customerId)
        {
            return _provider.GetCustomerData(customerId);
        }

        public int GetPoints(Guid customerId)
        {
            return _provider.GetPoints(customerId);
        }

        public Guid AddCustomer(string externalCode, int bonusPoints = 0)
        {
            return _provider.AddCustomer(externalCode, bonusPoints);
        }

        public bool AddCustomer(CustomerData customerData)
        {
            return _provider.AddCustomer(customerData);
        }

        public bool RemoveCustomer(Guid customerId)
        {
            return _provider.RemoveCustomer(customerId);
        }

        public bool AddPointForCustomer(Guid customerId, int points)
        {
            return _provider.AddPointForCustomer(customerId, points);
        }
    }
}
