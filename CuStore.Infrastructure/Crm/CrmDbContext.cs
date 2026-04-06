using Microsoft.EntityFrameworkCore;

namespace CuStore.Infrastructure.Crm;

public class CrmCustomer
{
    public Guid Id { get; set; }
    public string ExternalCode { get; set; } = string.Empty;
    public int Points { get; set; }
    public decimal Ratio { get; set; }
}

public class CrmDbContext(DbContextOptions<CrmDbContext> options) : DbContext(options)
{
    public DbSet<CrmCustomer> CustomerData => Set<CrmCustomer>();
}
