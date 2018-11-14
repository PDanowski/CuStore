using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.Domain.Extensions;

namespace CuStore.Domain.Concrete
{
    public class StoreRepository : IStoreRepository
    {
        private readonly IStoreContext _context;

        public StoreRepository(IStoreContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetOrdersWithItems()
        {
            return _context.Orders.Include(o => o.OrderItems);
        }

        public IEnumerable<Order> GetOrders()
        {
            return _context.Orders;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products;
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id.Equals(id));
        }

        public int GetProductsCount(int? categoryId = null)
        {
            if (categoryId.HasValue)
            {
                //return _context.Products
                //    .Where(p => p.Category.Id.Equals(categoryId.Value))
                //    .Count();
                return 2;
            }
            //return _context.Products.Count();
            return 12;
        }

        public IEnumerable<Product> GetProductsByCategory(int pageSize, int pageNumber, int? categoryId = null)
        {
            var test = new List<Product>
            {
                new Product {Id = 1, Name = "Product1", Price = 10, CategoryId = 2, Category = _context.Categories.FirstOrDefault(c => c.Id == 4)},
                new Product {Id = 2, Name = "Product2", Price = 20, CategoryId = 2, Category = _context.Categories.FirstOrDefault(c => c.Id == 4)},
                new Product {Id = 3, Name = "Product3", Price = 10, CategoryId = 3, Category = _context.Categories.FirstOrDefault(c => c.Id == 5)},
                new Product {Id = 4, Name = "Product4", Price = 25, CategoryId = 4, Category = _context.Categories.FirstOrDefault(c => c.Id == 6)},
                new Product {Id = 5, Name = "Product5", Price = 30, CategoryId = 5, Category = _context.Categories.FirstOrDefault(c => c.Id == 7)},
                new Product {Id = 6, Name = "Product6", Price = 10, CategoryId = 6, Category = _context.Categories.FirstOrDefault(c => c.Id == 8)},
                new Product {Id = 7, Name = "Product7", Price = 20, CategoryId = 7, Category = _context.Categories.FirstOrDefault(c => c.Id == 9)},
                new Product {Id = 8, Name = "Product8", Price = 15, CategoryId = 9, Category = _context.Categories.FirstOrDefault(c => c.Id == 11)},
                new Product {Id = 9, Name = "Product9", Price = 20, CategoryId = 10, Category = _context.Categories.FirstOrDefault(c => c.Id == 12)},
                new Product {Id = 10, Name = "Product10", Price = 35, CategoryId = 10, Category = _context.Categories.FirstOrDefault(c => c.Id == 12)},
                new Product {Id = 11, Name = "Product11", Price = 10, CategoryId = 11, Category = _context.Categories.FirstOrDefault(c => c.Id == 13)},
                new Product {Id = 12, Name = "Product12", Price = 25, CategoryId = 12, Category = _context.Categories.FirstOrDefault(c => c.Id == 14)}
            };

            if (categoryId.HasValue)
            {               
                var category = _context.Categories.FirstOrDefault(c => c.Id.Equals(categoryId.Value));
                if (category != null)
                {
                    var childCategories = _context.Categories.Where(c => c.ParentCategoryId.HasValue
                                                                         && c.ParentCategoryId.Value.Equals(categoryId.Value));

                    var categoriesToFilter = childCategories.Select(c => c.Id).ToList();
                    categoriesToFilter.Add(category.Id);

                    //return _context.Products
                    return test
                        .Where(p => p.CategoryId.In(categoriesToFilter))
                        .OrderBy(p => p.Id)
                        .Skip((pageNumber - 1) * pageSize).Take(pageSize);
                }
            }
            //return _context.Products
            return test
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
