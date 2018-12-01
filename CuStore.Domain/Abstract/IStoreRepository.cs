using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Entities;

namespace CuStore.Domain.Abstract
{
    public interface IStoreRepository
    {
        IEnumerable<Product> GetProducts();
        IEnumerable<Category> GetCategories();
        IEnumerable<Cart> GetCartWithItemsForUser(string userId);
        Product GetProductById(int productId);
        Category GetCategoryById(int id);
        IEnumerable<Product> GetProductsByCategory(int pageSize, int pageNumber, int? categoryId = null);
        int GetProductsCount(int? categoryId = null);
        bool AddCart(Cart cart);
        bool SaveCart(Cart cart);
        Cart GetCartByUserId(string userId);
        User GetUserById(string userId);
        bool RemoveCart(Cart cart);
        IEnumerable<ShippingMethod> GetShippingMethods();
    }
}
