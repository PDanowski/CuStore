using CuStore.Core.Abstractions;
using CuStore.Core.Entities;
using CuStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CuStore.Infrastructure.Repositories;

public class OrderRepository(StoreDbContext context) : IOrderRepository
{
    public int GetOrdersCount() => context.Orders.Count();

    public bool AddOrder(Order order)
    {
        context.Orders.Add(order);
        context.SaveChanges();
        return true;
    }

    public bool RemoveOrder(int orderId)
    {
        var order = context.Orders.SingleOrDefault(o => o.Id == orderId);
        if (order is null)
        {
            return false;
        }

        context.Orders.Remove(order);
        context.SaveChanges();
        return true;
    }

    public Order? GetOrderById(int orderId)
    {
        var order = context.Orders
            .Include(o => o.Cart)
            .ThenInclude(c => c!.CartItems)
            .ThenInclude(ci => ci.Product)
            .Include(o => o.Cart)
            .ThenInclude(c => c!.User)
            .ThenInclude(u => u!.UserAddress)
            .Include(o => o.ShippingMethod)
            .Include(o => o.ShippingAddress)
            .FirstOrDefault(o => o.Id == orderId);

        return order;
    }

    public IEnumerable<Order> GetOrdersByUser(string userId) =>
        context.Orders
            .Include(o => o.Cart)
            .ThenInclude(c => c!.CartItems)
            .ThenInclude(ci => ci.Product)
            .Include(o => o.Cart)
            .ThenInclude(c => c!.User)
            .ThenInclude(u => u!.UserAddress)
            .Include(o => o.ShippingMethod)
            .Include(o => o.ShippingAddress)
            .Where(x => x.Cart!.UserId.ToLower() == userId.ToLower())
            .ToList();

    public IEnumerable<Order> GetOrders(int pageSize, int pageNumber) =>
        context.Orders
            .Include(o => o.Cart)
            .ThenInclude(c => c!.User)
            .OrderBy(o => o.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

    public bool SaveOrder(Order order)
    {
        var existingOrder = context.Orders.SingleOrDefault(o => o.Id == order.Id);
        if (existingOrder is null)
        {
            return false;
        }

        context.Entry(existingOrder).CurrentValues.SetValues(order);
        context.SaveChanges();
        return true;
    }
}
