using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.Domain.Extensions;

namespace CuStore.Domain.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IStoreContext _context;

        public ProductRepository(IStoreContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetProducts(bool includeCategry = true)
        {
            return _context.Products.Include(p => p.Category).ToList();
        }

        public int GetProductsCountByPhrase(string phrase, int? categoryId = null)
        {
            if (string.IsNullOrEmpty(phrase))
            {
                throw new ArgumentException("Searching phrase can not be empty");
            }

            if (categoryId.HasValue)
            {
                return _context.Products
                    .Count(p => p.Name.Contains(phrase) && p.CategoryId.Equals(categoryId.Value));
            }
            return _context.Products.Count(p => p.Name.Contains(phrase));
        }

        public bool IsProductCodeUnique(string code)
        {
            var products =
                _context.Products.Where(p => p.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase));

            if (products.Any())
            {
                return false;
            }

            return true;
        }

        public IEnumerable<Product> GetProductsByCategory(int pageSize, int pageNumber, int? categoryId = null)
        {
            if (categoryId.HasValue)
            {
                var category = _context.Categories.FirstOrDefault(c => c.Id.Equals(categoryId.Value));
                if (category != null)
                {
                    var childCategories = _context.Categories.Where(c => c.ParentCategoryId.HasValue
                                                                         && c.ParentCategoryId.Value.Equals(categoryId.Value));

                    var categoriesToFilter = childCategories.Select(c => c.Id).ToList();
                    categoriesToFilter.Add(category.Id);

                    return _context.Products
                        .OrderBy(p => p.Id).ToList()
                        .Where(p => p.CategoryId.In(categoriesToFilter))
                        .Skip((pageNumber - 1) * pageSize).Take(pageSize);
                }
            }
            return _context.Products
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public IEnumerable<Product> GetProductsByPhrase(string phrase, int pageSize, int pageNumber, int? categoryId = null)
        {
            if (string.IsNullOrEmpty(phrase))
            {
                throw new ArgumentException("Searching phrase can not be empty");
            }

            if (categoryId.HasValue)
            {
                var category = _context.Categories.FirstOrDefault(c => c.Id.Equals(categoryId.Value));
                if (category != null)
                {
                    var childCategories = _context.Categories.Where(c => c.ParentCategoryId.HasValue
                                                                         && c.ParentCategoryId.Value.Equals(categoryId.Value));

                    var categoriesToFilter = childCategories.Select(c => c.Id).ToList();
                    categoriesToFilter.Add(category.Id);

                    return _context.Products
                        .Where(p => p.Name.Contains(phrase) && p.CategoryId.In(categoriesToFilter))
                        .OrderBy(p => p.Id).ToList()
                        .Skip((pageNumber - 1) * pageSize).Take(pageSize);
                }
            }
            return _context.Products
                .Where(p => p.Name.Contains(phrase))
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public int GetProductsCount(int? categoryId = null)
        {
            if (categoryId.HasValue)
            {
                return _context.Products
                    .Count(p => p.Category.Id.Equals(categoryId.Value));
            }
            return _context.Products.Count();
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.Id.Equals(productId));
        }

        public bool SaveProduct(Product product)
        {
            try
            {
                var existingProduct = _context.Products
                    .SingleOrDefault(p => p.Id == product.Id);

                // Update 
                _context.Entry(existingProduct).CurrentValues.SetValues(product);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }

        public bool AddProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }

        public bool RemoveProduct(int productId)
        {
            try
            {
                Product product = new Product { Id = productId };
                _context.Entry(product).State = EntityState.Deleted;
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
