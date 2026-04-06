using CuStore.Core.Entities;

namespace CuStore.Core.Abstractions;

public interface ICartRepository
{
    Cart? GetActiveCartWithItemsForUser(string userId);
    bool AddCart(Cart cart);
    bool SaveCart(Cart cart);
    Cart? GetCartByUserId(string userId);
    bool RemoveCart(Cart cart);
}
