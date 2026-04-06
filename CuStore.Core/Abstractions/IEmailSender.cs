using CuStore.Core.Entities;

namespace CuStore.Core.Abstractions;

public interface IEmailSender
{
    void ProcessOrder(Order order);
}
