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
        IEnumerable<Order> GetOrdersWithItems();
        IEnumerable<Order> GetOrders();
        IEnumerable<Product> GetProducts();
        IEnumerable<Category> GetCategories();
        Category GetCategoryById(int id);
        IEnumerable<Product> GetProductsByCategory(int pageSize, int pageNumber, int? categoryId = null);
        int GetProductsCount(int? categoryId = null);
    }
}
