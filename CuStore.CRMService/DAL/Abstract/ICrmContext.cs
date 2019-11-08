using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using CuStore.CRMService.DAL.Models;

namespace CuStore.CRMService.DAL.Abstract
{
    public interface ICrmContext
    {
        DbEntityEntry Entry(object model);
        DbSet<CustomerCrmData> CustomerData { get; }
        int SaveChanges();
    }
}
