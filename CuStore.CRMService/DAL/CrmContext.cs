using System.Data.Entity;
using CuStore.CRMService.DAL.Abstract;
using CuStore.CRMService.DAL.Models;

namespace CuStore.CRMService.DAL
{
    public class CrmContext : DbContext, ICrmContext
    {
        public CrmContext() : base("name=CrmService")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<CrmContext>());
        }

        public DbSet<CustomerCrmData> CustomerData { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<CrmContext>(new CreateDatabaseIfNotExists<CrmContext>());
        }
    }
}