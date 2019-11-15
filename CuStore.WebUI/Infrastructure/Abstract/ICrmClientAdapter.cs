using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Entities;

namespace CuStore.WebUI.Infrastructure.Abstract
{
    public interface ICrmClientAdapter
    {
        bool CreateDataForCustomers(IEnumerable<User> users);
    }
}
