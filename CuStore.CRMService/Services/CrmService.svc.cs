using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.ServiceModel;
using CuStore.CRMService.DAL.Abstract;
using CuStore.CRMService.DataContracts;
using CuStore.CRMService.FaultExceptions;
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
            try
            {
                return _provider.GetCustomerData(customerId);
            }
            catch (Exception ex)
            {
                throw new CrmInternalFaultException(ex.Message);
            }
        }

        public int GetPoints(Guid customerId)
        {           
            try
            {
                return _provider.GetPoints(customerId);
            }
            catch (Exception ex)
            {
                throw new CrmInternalFaultException(ex.Message);
            }
        }

        public Guid AddCustomer(string externalCode, int bonusPoints = 0)
        {    
            try
            {
                return _provider.AddCustomer(externalCode, bonusPoints);
            }
            catch (Exception ex)
            {
                throw new CrmInternalFaultException(ex.Message);
            }
        }

        public bool AddCustomer(CustomerData customerData)
        {           
            try
            {
                return _provider.AddCustomer(customerData);
            }
            catch (Exception ex)
            {
                throw new CrmInternalFaultException(ex.Message);
            }

        }

        public bool RemoveCustomer(Guid customerId)
        {       
            try
            {
                return _provider.RemoveCustomer(customerId);
            }
            catch (Exception ex)
            {
                throw new CrmInternalFaultException(ex.Message);
            }
        }

        public bool AddPointForCustomer(Guid customerId, int points)
        {           
            try
            {
                return _provider.AddPointForCustomer(customerId, points);
            }
            catch (Exception ex)
            {
                throw new CrmInternalFaultException(ex.Message);
            }
        }
    }
}
