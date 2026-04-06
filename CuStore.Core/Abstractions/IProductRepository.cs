using CuStore.Core.Entities;

namespace CuStore.Core.Abstractions;

public interface IProductRepository
{
    IEnumerable<Product> GetProducts(bool includeCategry = true);
    Product? GetProductById(int productId);
    bool SaveProduct(Product product);
    bool AddProduct(Product product);
    bool RemoveProduct(int productId);
    IEnumerable<Product> GetProductsByCategory(int pageSize, int pageNumber, int? categoryId = null);
    IEnumerable<Product> GetProductsByPhrase(string phrase, int pageSize, int pageNumber, int? categoryId = null);
    int GetProductsCount(int? categoryId = null);
    int GetProductsCountByPhrase(string phrase, int? categoryId = null);
    bool IsProductCodeUnique(string code);
}
