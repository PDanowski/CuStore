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

namespace CuStore.Domain.Concrete
{
    public class StoreRepository : IStoreRepository
    {
        private readonly IStoreContext _context;

        public StoreRepository(IStoreContext context)
        {
            _context = context;
        }

        public IEnumerable<Cart> GetCartWithItemsForUser(string userId)
        {
            return _context.Carts
                .Where(c => c.UserId.Equals(userId))
                .Include(c => c.CartItems);
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories;
        }

        public bool AddCart(Cart cart)
        {
            try
            {
                _context.Carts.Add(cart);
                _context.SaveChanges();

                var existingParent = _context.Carts
                    .SingleOrDefault(p => p.Id == cart.Id);

                if (existingParent != null)
                {
                    // Update and Insert children
                    foreach (var cartItem in cart.CartItems)
                    {
                        // Insert child
                        var newChild = new CartItem
                        {
                            ProductId = cartItem.ProductId,
                            CartId = cartItem.CartId,
                            Quantity = cartItem.Quantity
                        };
                        existingParent.CartItems.Add(newChild);
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }

        public bool SaveCart(Cart cart)
        {
            //_context.Carts.Attach(cart);
            //_context.Entry(cart).State = EntityState.Modified;

            try
            {
                var existingCart = _context.Carts
                    .Where(c => c.Id == cart.Id)
                    .Include(c => c.CartItems)
                    .SingleOrDefault();

                if (existingCart != null)
                {
                    // Update parent
                    _context.Entry(existingCart).CurrentValues.SetValues(cart);

                    // Delete children
                    foreach (var existingCartItem in existingCart.CartItems.ToList())
                    {
                        if (!cart.CartItems.Any(c => c.Id == existingCartItem.Id))
                            _context.CartItems.Remove(existingCartItem);
                    }

                    // Update and Insert children
                    foreach (var cartItem in cart.CartItems)
                    {
                        var existingCartItem = existingCart.CartItems
                            .SingleOrDefault(c => c.Id == cartItem.Id);

                        if (existingCartItem != null)
                            // Update child
                            _context.Entry(existingCartItem).CurrentValues.SetValues(cartItem);
                        else
                        {
                            // Insert child
                            var newChild = new CartItem
                            {
                                ProductId = cartItem.ProductId,
                                CartId = cartItem.CartId,
                                Quantity = cartItem.Quantity
                            };
                            existingCart.CartItems.Add(newChild);
                        }
                    }

                    _context.SaveChanges();
                    
                }
                return true;
                //_context.SaveChanges();

            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }

        public bool RemoveCart(Cart cart)
        {
            _context.Carts.Attach(cart);
            _context.Entry(cart).State = EntityState.Deleted;

            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }

        public User GetUserById(string userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id.Equals(userId));
        }

        public Cart GetCartByUserId(string userId)
        {
            return _context.Carts.FirstOrDefault(c => c.UserId.Equals(userId));
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products;
        }

        public IEnumerable<ShippingMethod> GetShippingMethods()
        {
            return _context.ShippingMethods;
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.Id.Equals(productId));
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id.Equals(id));
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

        public IEnumerable<Product> GetProductsByCategory(int pageSize, int pageNumber, int? categoryId = null)
        {
            //var test = new List<Product>
            //{
            //    new Product {Id = 1, Name = "Product1", Price = 10, CategoryId = 3, Category = _context.Categories.FirstOrDefault(c => c.Id == 3)},
            //    new Product {Id = 2, Name = "Product2", Price = 20, CategoryId = 3, Category = _context.Categories.FirstOrDefault(c => c.Id == 3)},
            //    new Product {Id = 3, Name = "Product3", Price = 10, CategoryId = 4, Category = _context.Categories.FirstOrDefault(c => c.Id == 4)},
            //    new Product {Id = 4, Name = "Product4", Price = 25, CategoryId = 5, Category = _context.Categories.FirstOrDefault(c => c.Id == 5)},
            //    new Product {Id = 5, Name = "Product5", Price = 30, CategoryId = 6, Category = _context.Categories.FirstOrDefault(c => c.Id == 6)},
            //    new Product {Id = 6, Name = "Product6", Price = 10, CategoryId = 7, Category = _context.Categories.FirstOrDefault(c => c.Id == 7)},
            //    new Product {Id = 7, Name = "Product7", Price = 20, CategoryId = 16, Category = _context.Categories.FirstOrDefault(c => c.Id == 16)},
            //    new Product {Id = 8, Name = "Product8", Price = 15, CategoryId = 9, Category = _context.Categories.FirstOrDefault(c => c.Id == 9)},
            //    new Product {Id = 9, Name = "Product9", Price = 20, CategoryId = 10, Category = _context.Categories.FirstOrDefault(c => c.Id == 10)},
            //    new Product {Id = 10, Name = "Product10", Price = 35, CategoryId = 10, Category = _context.Categories.FirstOrDefault(c => c.Id == 10)},
            //    new Product {Id = 11, Name = "Product11", Price = 10, CategoryId = 11, Category = _context.Categories.FirstOrDefault(c => c.Id == 11)},
            //    new Product {Id = 12, Name = "Product12", Price = 25, CategoryId = 12, Category = _context.Categories.FirstOrDefault(c => c.Id == 12)}
            //};

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
                        .Where(p => p.CategoryId.In(categoriesToFilter))
                        .OrderBy(p => p.Id)
                        .Skip((pageNumber - 1) * pageSize).Take(pageSize);
                }
            }
            return _context.Products
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
