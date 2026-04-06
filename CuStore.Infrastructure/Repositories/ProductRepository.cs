using CuStore.Core.Abstractions;
using CuStore.Core.Entities;
using CuStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CuStore.Infrastructure.Repositories;

public class ProductRepository(StoreDbContext context) : IProductRepository
{
    public IEnumerable<Product> GetProducts(bool includeCategry = true)
    {
        var query = context.Products.AsQueryable();
        if (includeCategry)
        {
            query = query.Include(p => p.Category);
        }

        return query.ToList();
    }

    public Product? GetProductById(int productId) =>
        context.Products.FirstOrDefault(p => p.Id == productId);

    public bool SaveProduct(Product product)
    {
        var existingProduct = context.Products.SingleOrDefault(p => p.Id == product.Id);
        if (existingProduct is null)
        {
            return false;
        }

        context.Entry(existingProduct).CurrentValues.SetValues(product);
        context.SaveChanges();
        return true;
    }

    public bool AddProduct(Product product)
    {
        context.Products.Add(product);
        context.SaveChanges();
        return true;
    }

    public bool RemoveProduct(int productId)
    {
        var existingProduct = context.Products.SingleOrDefault(p => p.Id == productId);
        if (existingProduct is null)
        {
            return false;
        }

        context.Products.Remove(existingProduct);
        context.SaveChanges();
        return true;
    }

    public IEnumerable<Product> GetProductsByCategory(int pageSize, int pageNumber, int? categoryId = null)
    {
        var query = context.Products.AsQueryable();

        if (categoryId.HasValue)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == categoryId.Value);
            if (category is not null)
            {
                var categoriesToFilter = context.Categories
                    .Where(c => c.ParentCategoryId == category.Id)
                    .Select(c => c.Id)
                    .ToList();
                categoriesToFilter.Add(category.Id);

                query = query.Where(p => categoriesToFilter.Contains(p.CategoryId));
            }
        }

        return query
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public IEnumerable<Product> GetProductsByPhrase(string phrase, int pageSize, int pageNumber, int? categoryId = null)
    {
        if (string.IsNullOrWhiteSpace(phrase))
        {
            throw new ArgumentException("Searching phrase can not be empty");
        }

        var query = context.Products.Where(p => p.Name.Contains(phrase));

        if (categoryId.HasValue)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == categoryId.Value);
            if (category is not null)
            {
                var categoriesToFilter = context.Categories
                    .Where(c => c.ParentCategoryId == category.Id)
                    .Select(c => c.Id)
                    .ToList();
                categoriesToFilter.Add(category.Id);
                query = query.Where(p => categoriesToFilter.Contains(p.CategoryId));
            }
        }

        return query
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public int GetProductsCount(int? categoryId = null) =>
        categoryId.HasValue
            ? context.Products.Count(p => p.CategoryId == categoryId.Value)
            : context.Products.Count();

    public int GetProductsCountByPhrase(string phrase, int? categoryId = null)
    {
        if (string.IsNullOrWhiteSpace(phrase))
        {
            throw new ArgumentException("Searching phrase can not be empty");
        }

        return categoryId.HasValue
            ? context.Products.Count(p => p.Name.Contains(phrase) && p.CategoryId == categoryId.Value)
            : context.Products.Count(p => p.Name.Contains(phrase));
    }

    public bool IsProductCodeUnique(string code) =>
        !context.Products.Any(p => p.Code.ToLower() == code.ToLower());
}
