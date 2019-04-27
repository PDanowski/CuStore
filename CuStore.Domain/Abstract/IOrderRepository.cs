using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Entities;

namespace CuStore.Domain.Abstract
{
    public interface IOrderRepository
    {
        int GetOrdersCount();
        bool AddOrder(Order order);
        bool RemoveOrder(int orderId);
        Order GetOrderById(int orderId);
        IEnumerable<Order> GetOrdersByUser(string userId);
        IEnumerable<Order> GetOrders(int pageSize, int pageNumber);
        bool SaveOrder(Order order);
    }
}
