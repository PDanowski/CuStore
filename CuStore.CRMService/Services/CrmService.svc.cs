using System;
using CuStore.CRMService.DataContracts;
using CuStore.CRMService.OperationContracts;

namespace CuStore.CRMService.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class CrmService : ICrmService
    {
        public CustomerData GetCustomerData(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public int GetPoints(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public Guid AddCustomer(string externalCode, int bonusPoints)
        {
            throw new NotImplementedException();
        }

        public bool RemoveCustomer(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public bool AddPointForCustomer(Guid customerId, int points)
        {
            throw new NotImplementedException();
        }
    }
}
