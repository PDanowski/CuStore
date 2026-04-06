using CuStore.Core.Abstractions;
using CuStore.Core.Entities;
using CuStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CuStore.Infrastructure.Repositories;

public class CartRepository(StoreDbContext context) : ICartRepository
{
    public Cart? GetActiveCartWithItemsForUser(string userId) =>
        context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .Include(c => c.User)
            .FirstOrDefault(c => c.UserId == userId && c.OrderId == null);

    public Cart? GetCartByUserId(string userId) =>
        context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefault(c => c.UserId == userId && c.OrderId == null);

    public bool AddCart(Cart cart)
    {
        context.Carts.Add(cart);
        context.SaveChanges();
        return true;
    }

    public bool SaveCart(Cart cart)
    {
        var existingCart = context.Carts
            .Include(c => c.CartItems)
            .SingleOrDefault(c => c.Id == cart.Id);

        if (existingCart is null)
        {
            return false;
        }

        context.Entry(existingCart).CurrentValues.SetValues(cart);

        foreach (var existingCartItem in existingCart.CartItems.ToList())
        {
            if (!cart.CartItems.Any(c => c.Id == existingCartItem.Id))
            {
                context.CartItems.Remove(existingCartItem);
            }
        }

        foreach (var cartItem in cart.CartItems)
        {
            var existingCartItem = existingCart.CartItems
                .SingleOrDefault(c => c.Id == cartItem.Id && c.Id != default);

            if (existingCartItem is not null)
            {
                context.Entry(existingCartItem).CurrentValues.SetValues(cartItem);
            }
            else
            {
                existingCart.CartItems.Add(new CartItem
                {
                    ProductId = cartItem.ProductId,
                    CartId = cart.Id,
                    Quantity = cartItem.Quantity,
                });
            }
        }

        context.SaveChanges();
        return true;
    }

    public bool RemoveCart(Cart cart)
    {
        context.Carts.Remove(cart);
        context.SaveChanges();
        return true;
    }
}
