using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Entities;

namespace CuStore.Domain.Abstract
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts(bool includeCategry = true);
        Product GetProductById(int productId);
        bool SaveProduct(Product product);
        bool AddProduct(Product product);
        bool RemoveProduct(int productId);
        IEnumerable<Product> GetProductsByCategory(int pageSize, int pageNumber, int? categoryId = null);
        int GetProductsCount(int? categoryId = null);
        bool IsProductCodeUnique(string code);
    }
}
