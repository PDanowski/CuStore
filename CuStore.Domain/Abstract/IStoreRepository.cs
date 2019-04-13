using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CuStore.Domain.Entities;

namespace CuStore.Domain.Abstract
{
    public interface IStoreRepository
    {
        IEnumerable<Product> GetProducts(bool includeCategry = true);
        IEnumerable<Category> GetCategories();
        IEnumerable<Category> GetParentCategories();
        Cart GetActiveCartWithItemsForUser(string userId);
        Product GetProductById(int productId);
        bool SaveProduct(Product product);
        bool AddProduct(Product product);
        bool RemoveProduct(int productId);
        bool SaveCategory(Category category);
        bool AddCategory(Category category);
        bool RemoveCategory(int categoryId);
        bool SaveShippingMethod(ShippingMethod shippingMethod);
        bool AddShippingMethod(ShippingMethod shippingMethod);
        bool RemoveShippingMethod(int shippingMethodId);
        Category GetCategoryById(int id);
        IEnumerable<Product> GetProductsByCategory(int pageSize, int pageNumber, int? categoryId = null);
        int GetProductsCount(int? categoryId = null);
        int GetOrdersCount();
        bool AddCart(Cart cart);
        bool SaveCart(Cart cart);
        Cart GetCartByUserId(string userId);
        User GetUserById(string userId);
        bool RemoveCart(Cart cart);
        IEnumerable<ShippingMethod> GetShippingMethods();
        bool AddOrder(Order order);
        bool RemoveOrder(int orderId);
        Order GetOrderById(int orderId);
        ShippingMethod GetShippingMethodById(int id);
        IEnumerable<Order> GetOrders(int pageSize, int pageNumber);
        bool SaveOrder(Order order);
        bool IsProductCodeUnique(string code);
    }
}
