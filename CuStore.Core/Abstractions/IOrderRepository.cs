using CuStore.Core.Entities;

namespace CuStore.Core.Abstractions;

public interface IOrderRepository
{
    int GetOrdersCount();
    bool AddOrder(Order order);
    bool RemoveOrder(int orderId);
    Order? GetOrderById(int orderId);
    IEnumerable<Order> GetOrdersByUser(string userId);
    IEnumerable<Order> GetOrders(int pageSize, int pageNumber);
    bool SaveOrder(Order order);
}
