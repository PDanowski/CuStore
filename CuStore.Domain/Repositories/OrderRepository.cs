using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;

namespace CuStore.Domain.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IStoreContext _context;

        public OrderRepository(IStoreContext context)
        {
            _context = context;
        }

        public bool AddOrder(Order order)
        {
            try
            {
                _context.Orders.Add(order);

                //var existingCart = _context.Carts
                //    .Where(c => c.Id == order.Cart.Id)
                //    .Include(c => c.CartItems)
                //    .SingleOrDefault();
                //order.Cart.OrderId = order.Id;
                //_context.Entry(existingCart).CurrentValues.SetValues(order.Cart);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }

        public bool RemoveOrder(int orderId)
        {
            try
            {
                Order order = new Order { Id = orderId };
                _context.Entry(order).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }

        public int GetOrdersCount()
        {
            return _context.Orders.Count();
        }


        public IEnumerable<Order> GetOrders(int pageSize, int pageNumber)
        {
            return _context.Orders
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Include(o => o.Cart)
                .Include(o => o.Cart.User)
                .ToList();
        }

        public Order GetOrderById(int orderId)
        {
            var order = _context.Orders
                .Where(o => o.Id.Equals(orderId))
                .Include(o => o.Cart)
                .Include(o => o.Cart.CartItems)
                .Include(o => o.Cart.User)
                .Include(o => o.Cart.User.UserAddress)
                .Include(o => o.ShippingMethod)
                .Include(o => o.ShippingAddress)
                .FirstOrDefault();

            if (order != null)
            {
                foreach (var item in order.Cart.CartItems)
                {
                    item.Product = _context.Products.FirstOrDefault(p => p.Id.Equals(item.ProductId));
                }
            }
            return order;
        }

        public IEnumerable<Order> GetOrdersByUser(string userId)
        {
            var orders = _context.Orders
                .Include(o => o.Cart)
                .Include(o => o.Cart.CartItems)
                .Include(o => o.Cart.User)
                .Include(o => o.Cart.User.UserAddress)
                .Include(o => o.ShippingMethod)
                .Include(o => o.ShippingAddress)
                .Where(x => x.Cart.UserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase));

            if (orders.Any())
            {
                foreach (var order in orders)
                {
                    foreach (var item in order.Cart.CartItems)
                    {
                        item.Product = _context.Products.FirstOrDefault(p => p.Id.Equals(item.ProductId));
                    }
                }
            }
            return orders;
        }

        public bool SaveOrder(Order order)
        {
            try
            {
                var existingProduct = _context.Orders
                    .SingleOrDefault(o => o.Id == order.Id);

                // Update 
                _context.Entry(existingProduct).CurrentValues.SetValues(order);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }
    }
}
