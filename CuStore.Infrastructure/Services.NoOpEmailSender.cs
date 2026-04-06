using CuStore.Core.Abstractions;
using CuStore.Core.Entities;

namespace CuStore.Infrastructure.Services;

public class NoOpEmailSender : IEmailSender
{
    public void ProcessOrder(Order order)
    {
        // Placeholder mail sender for migration phase; wire real provider later.
    }
}
