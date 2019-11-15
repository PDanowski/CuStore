using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CuStore.Domain.Entities;
using CuStore.WebUI.Infrastructure.Abstract;

namespace CuStore.WebUI.Infrastructure.Implementations
{
    public class CrmClientAdapter : ICrmClientAdapter
    {
        private readonly ICrmClient _client;
        private readonly ILogger _logger;

        public CrmClientAdapter(ICrmClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;
        }

        public bool CreateDataForCustomers(IEnumerable<User> users)
        {
            try
            {
                foreach (var user in users)
                {
                    var guid = _client.CreateCustomerData(user.UserName, 0);
                    user.CrmGuid = guid;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error during creating CRM data for user");
                _logger.LogException(ex);
                return false;
            }
        }
    }
}