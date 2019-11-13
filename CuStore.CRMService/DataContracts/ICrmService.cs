using System;
using System.ServiceModel;
using CuStore.CRMService.OperationContracts;

namespace CuStore.CRMService.DataContracts
{
    [ServiceContract]
    public interface ICrmService
    {

        [OperationContract]
        CustomerData GetCustomerData(Guid customerId);

        [OperationContract]
        int GetPoints(Guid customerId);

        [OperationContract]
        Guid AddCustomer(string externalCode, int bonusPoints = 0);

        [OperationContract(Name = "AddCustomerData")]
        bool AddCustomer(CustomerData customerData);

        [OperationContract]
        bool RemoveCustomer(Guid customerId);

        [OperationContract]
        bool AddPointForCustomer(Guid customerId, int points);

    }

}
