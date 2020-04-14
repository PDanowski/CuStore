using System;
using System.ServiceModel;
using CuStore.CRMService.FaultExceptions;
using CuStore.CRMService.OperationContracts;

namespace CuStore.CRMService.DataContracts
{
    [ServiceContract]
    public interface ICrmService
    {

        [OperationContract]
        [FaultContract(typeof(CrmInternalFaultException))]
        CustomerData GetCustomerData(Guid customerId);

        [OperationContract]
        int GetPoints(Guid customerId);

        [OperationContract]
        [FaultContract(typeof(CrmInternalFaultException))]
        Guid AddCustomer(string externalCode, int bonusPoints = 0);

        [OperationContract(Name = "AddCustomerData")]
        [FaultContract(typeof(CrmInternalFaultException))]
        bool AddCustomer(CustomerData customerData);

        [OperationContract]
        [FaultContract(typeof(CrmInternalFaultException))]
        bool RemoveCustomer(Guid customerId);

        [OperationContract]
        [FaultContract(typeof(CrmInternalFaultException))]
        bool AddPointForCustomer(Guid customerId, int points);

    }

}
