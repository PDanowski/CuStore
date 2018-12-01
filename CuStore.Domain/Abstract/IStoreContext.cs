using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Entities;

namespace CuStore.Domain.Abstract
{
    public interface IStoreContext
    {
        DbEntityEntry Entry(object model);
        DbSet<Product> Products { get; }
        DbSet<Category> Categories { get; }
        DbSet<Order> Orders { get; }
        DbSet<Cart> Carts { get; }
        DbSet<User> Users { get; }
        DbSet<Address> Addresses { get; }
        DbSet<CartItem> CartItems { get; }
        DbSet<ShippingMethod> ShippingMethods { get; }

        int SaveChanges();
    }
}
