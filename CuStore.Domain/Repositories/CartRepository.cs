using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;

namespace CuStore.Domain.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IStoreContext _context;

        public CartRepository(IStoreContext context)
        {
            _context = context;
        }

        public Cart GetActiveCartWithItemsForUser(string userId)
        {
            return _context.Carts
                .Where(c => c.UserId.Equals(userId) && !c.OrderId.HasValue)
                .Include(c => c.CartItems)
                .Include(c => c.User)
                .FirstOrDefault();
        }

        public Cart GetCartByUserId(string userId)
        {
            return _context.Carts
                .Include(c => c.CartItems.Select(ci => ci.Product))
                .FirstOrDefault(c => c.UserId.Equals(userId));
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
            try
            {
                //_context.Carts.Attach(cart);
                //_context.Entry(cart).State = EntityState.Modified;

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
                            .SingleOrDefault(c => c.Id == cartItem.Id && c.Id != default(int));

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
                    return true;
                }

                return false;
                //_context.SaveChanges();
                //return true;


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
    }
}
