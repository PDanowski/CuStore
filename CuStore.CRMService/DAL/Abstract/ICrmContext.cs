using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.CRMService.DAL.Models;

namespace CuStore.CRMService.DAL.Abstract
{
    public interface ICrmContext
    {
        DbSet<CustomerData> CustomerData { get; }
        int SaveChanges();
    }
}
