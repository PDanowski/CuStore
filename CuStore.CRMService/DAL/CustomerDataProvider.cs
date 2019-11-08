using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using AutoMapper;
using CuStore.CRMService.DAL.Abstract;
using CuStore.CRMService.DAL.Models;
using CuStore.CRMService.OperationContracts;

namespace CuStore.CRMService.DAL
{
    public class CustomerDataProvider : ICustomerDataProvider
    {
        private readonly ICrmContext _context;
        private readonly IMapper _mapper;

        public CustomerDataProvider(ICrmContext context)
        {
            _context = context;

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<CustomerData, CustomerCrmData>();
                cfg.CreateMap<CustomerCrmData, CustomerData>();
            });

            _mapper = config.CreateMapper();
        }

        public CustomerData GetCustomerData(Guid customerId)
        {
            var customerData = _context.CustomerData.FirstOrDefault(cd => cd.Id == customerId);

            if (customerData == null)
            {
                throw new ArgumentException($"Can't find customer with GUID: {customerId}");
            }

            return _mapper.Map<CustomerCrmData, CustomerData>(customerData);
        }

        public int GetPoints(Guid customerId)
        {
            var customerData = _context.CustomerData.FirstOrDefault(cd => cd.Id == customerId);

            if (customerData == null)
            {
                throw new ArgumentException($"Can't find customer with GUID: {customerId}");
            }

            return customerData.Points;
        }

        public Guid AddCustomer(string externalCode, int bonusPoints = 0)
        {
            Guid guid = Guid.NewGuid();

            _context.CustomerData.Add(new CustomerCrmData
            {
                Id = guid,
                ExternalCode = externalCode,
                Points = bonusPoints,
                Ratio = 1.0M
            });

            if (_context.SaveChanges() == 1)
            {
                return guid;
            }

            throw new DbUpdateException("Error during adding new CustomerData.");
        }

        public bool AddCustomer(CustomerData customerData)
        {
            CustomerCrmData customerCrmData = _mapper.Map<CustomerData, CustomerCrmData>(customerData);

            _context.CustomerData.Add(customerCrmData);

            if (_context.SaveChanges() == 1)
            {
                return true;
            }

            throw new DbUpdateException("Error during adding new CustomerData.");
        }

        public bool RemoveCustomer(Guid customerId)
        {
            var customerData = _context.CustomerData.FirstOrDefault(cd => cd.Id == customerId);

            if (customerData == null)
            {
                throw new ArgumentException($"Can't find customer with GUID: {customerId}");
            }

            _context.CustomerData.Remove(customerData);

            if (_context.SaveChanges() == 1)
            {
                return true;
            }

            throw new DbUpdateException("Error during adding new CustomerData.");
        }

        public bool AddPointForCustomer(Guid customerId, int points)
        {
            var customerData = _context.CustomerData.FirstOrDefault(cd => cd.Id == customerId);

            if (customerData == null)
            {
                throw new ArgumentException($"Can't find customer with GUID: {customerId}");
            }

            // Update 
            customerData.Points = points;
            _context.Entry(customerData).Property(nameof(customerData.Points)).IsModified = true;

            if (_context.SaveChanges() == 1)
            {
                return true;
            }

            throw new DbUpdateException("Error during updating new CustomerData.");
        }
    }
}