using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuStore.Domain.Entities
{
    public class Cart
    {
        public Cart()
        {
            CartItems = new HashSet<CartItem>();
        }

        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int? OrderId { get; set; }

        public ICollection<CartItem> CartItems { get; set; }

        public Order Order { get; set; }
        public User User { get; set; }


        public void AddProduct(Product product, int quantity)
        {
            CartItem existingItem = CartItems.FirstOrDefault(p => p.ProductId.Equals(product.Id));

            if (existingItem == null)
            {
                var item = new CartItem
                {
                    Product = product,
                    ProductId = product.Id,
                    Quantity = quantity,
                    CartId = Id
                };
                CartItems.Add(item);
            }
            else
            {
                existingItem.Quantity += quantity;
            }
        }

        public bool ChangeProductQuantity(int productId, int quantity)
        {
            CartItem existingItem = CartItems.FirstOrDefault(p => p.ProductId.Equals(productId));

            if (existingItem == null)
            {
                return false;
            }

            existingItem.Quantity = quantity;
            return true;
        }

        public void RemoveProduct(int productId)
        {
            var item = CartItems.FirstOrDefault(i => i.ProductId.Equals(productId));
            CartItems.Remove(item);
        }

        public decimal GetValue()
        {
            return CartItems.Sum(i => i.Quantity * i.Product.Price);
        }

        public void Clear()
        {
            CartItems.Clear();
        }
    }
}
