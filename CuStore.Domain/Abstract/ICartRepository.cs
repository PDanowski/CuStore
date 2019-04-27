using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Entities;

namespace CuStore.Domain.Abstract
{
    public interface ICartRepository
    {
        Cart GetActiveCartWithItemsForUser(string userId);
        bool AddCart(Cart cart);
        bool SaveCart(Cart cart);
        Cart GetCartByUserId(string userId);
        bool RemoveCart(Cart cart);

    }
}
